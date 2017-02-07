﻿//
//  Copyright (C) 2017 Fluendo S.A.
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
//
using System;
using System.Collections.Generic;
using System.Linq;
using VAS.Core.Common;
using VAS.Core.Events;
using VAS.Core.Hotkeys;
using VAS.Core.Interfaces.GUI;
using VAS.Core.Interfaces.MVVMC;
using VAS.Core.MVVMC;
using VAS.Core.Store;
using VAS.Core.ViewModel;

namespace VAS.Services.Controller
{
	public abstract class TaggingController : ControllerBase
	{
		protected ProjectVM project;
		protected VideoPlayerVM videoPlayer;

		/// <summary>
		/// Gets or sets the video player view model
		/// </summary>
		/// <value>The video player.</value>
		protected VideoPlayerVM VideoPlayer {
			get {
				return videoPlayer;
			}

			set {
				if (videoPlayer != null) {
					videoPlayer.PropertyChanged -= HandleVideoPlayerPropertyChanged;
				}
				videoPlayer = value;
				if (videoPlayer != null) {
					videoPlayer.PropertyChanged += HandleVideoPlayerPropertyChanged;
				}
			}
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		public override void Start ()
		{
			base.Start ();
			App.Current.EventsBroker.Subscribe<ClickedPCardEvent> (HandleClickedPCardEvent);
			App.Current.EventsBroker.Subscribe<NewTagEvent> (HandleNewTagEvent);
		}

		/// <summary>
		/// Stop this instance.
		/// </summary>
		public override void Stop ()
		{
			base.Stop ();
			App.Current.EventsBroker.Unsubscribe<ClickedPCardEvent> (HandleClickedPCardEvent);
			App.Current.EventsBroker.Unsubscribe<NewTagEvent> (HandleNewTagEvent);
		}

		/// <summary>
		/// Sets the view model.
		/// </summary>
		/// <param name="viewModel">View model.</param>
		public override void SetViewModel (IViewModel viewModel)
		{
			project = (ProjectVM)(viewModel as dynamic);
			VideoPlayer = (VideoPlayerVM)(viewModel as dynamic);
		}

		/// <summary>
		/// Gets the default key actions.
		/// </summary>
		/// <returns>The default key actions.</returns>
		public override IEnumerable<KeyAction> GetDefaultKeyActions ()
		{
			return Enumerable.Empty<KeyAction> ();
		}

		/// <summary>
		/// Handles when a Participant Card is clicked.
		/// </summary>
		/// <param name="e">Event.</param>
		protected void HandleClickedPCardEvent (ClickedPCardEvent e)
		{
			if (e.ClickedPlayer != null) {
				if (e.Modifier == ButtonModifier.Control) {
					e.ClickedPlayer.Tagged = !e.ClickedPlayer.Locked;
					e.ClickedPlayer.Locked = !e.ClickedPlayer.Locked;
				} else {
					if (!e.ClickedPlayer.Locked) {
						e.ClickedPlayer.Tagged = !e.ClickedPlayer.Tagged;
					}
				}
			}

			// Without the Shift modifier, unselect the rest of players that are not locked.
			if (e.Modifier != ButtonModifier.Shift) {
				foreach (PlayerVM player in project.Players) {
					if (player != e.ClickedPlayer && !player.Locked) {
						player.Tagged = false;
					}
				}
			}

			// Right now we don't care about selections and moving pcards
		}

		protected abstract TimelineEvent CreateTimelineEvent (EventType type, Time start, Time stop,
															  Time eventTime, Image miniature);

		void HandleNewTagEvent (NewTagEvent e)
		{
			if (project == null) {
				return;
			}

			var play = CreateTimelineEvent (e.EventType, e.Start, e.Stop, e.EventTime, null);

			AddPlayersToEvent (play);

			App.Current.EventsBroker.Publish (
				new NewDashboardEvent {
					TimelineEvent = play,
					DashboardButton = e.Button,
					Edit = false,
					DashboardButtons = null,
					ProjectId = project.Model.ID
				}
			);
			Reset ();
		}

		void AddPlayersToEvent (TimelineEvent play)
		{
			var players = project.Players.Where (p => p.Tagged);
			foreach (var playerVM in players) {
				play.Players.Add (playerVM.Model);
			}

			var teams = project.Teams.Where (team => players.Any (player => team.Contains (player))).Select (vm => vm.Model);
			play.Teams.AddRange (teams);
		}

		/// <summary>
		/// Resets all pCards.
		/// </summary>
		void Reset ()
		{
			foreach (PlayerVM player in project.Players) {
				if (!player.Locked) {
					player.Tagged = false;
				}
			}
		}

		void HandleVideoPlayerPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender == videoPlayer && e.PropertyName == "CurrentTime") {
				SetVideoCurrentTimeToTimerButtons ();
			}
		}

		void SetVideoCurrentTimeToTimerButtons ()
		{
			project.Dashboard.CurrentTime = VideoPlayer.CurrentTime;
			foreach (var timerVM in project.Dashboard.ViewModels.OfType<TimerButtonVM> ()) {
				timerVM.CurrentTime = VideoPlayer.CurrentTime;
			}

			foreach (var timedVM in project.Dashboard.ViewModels.OfType<TimedDashboardButtonVM> ()) {
				timedVM.CurrentTime = VideoPlayer.CurrentTime;
			}
		}
	}
}
