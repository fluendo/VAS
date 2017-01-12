﻿//
//  Copyright (C) 2016 Fluendo S.A.
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VAS.Core.Events;
using VAS.Core.Hotkeys;
using VAS.Core.Interfaces.MVVMC;
using VAS.Core.MVVMC;
using VAS.Core.Store;
using VAS.Core.ViewModel;

namespace VAS.Services.Controller
{
	/// <summary>
	/// Events controller, base class of the Events Controller.
	/// </summary>
	public class EventsController<TModel, TViewModel> : DisposableBase, IController
		where TModel : TimelineEvent
		where TViewModel : TimelineEventVM<TModel>, new()
	{
		VideoPlayerVM playerVM;
		TimelineVM viewModel;

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			Stop ();
		}

		public VideoPlayerVM PlayerVM {
			get {
				return playerVM;
			}
			protected set {
				if (playerVM != null) {
					playerVM.PropertyChanged -= HandlePlayerVMPropertyChanged;
				}
				playerVM = value;
				if (playerVM != null) {
					playerVM.PropertyChanged += HandlePlayerVMPropertyChanged;
				}
			}
		}

		public virtual TimelineVM ViewModel {
			get {
				return viewModel;
			}
			protected set {
				if (viewModel != null) {
					viewModel.Filters.PropertyChanged -= HandlePropertyChanged;
				}
				viewModel = value;
				if (viewModel != null) {
					HandleFiltersChanged ();
					viewModel.Filters.PropertyChanged += HandlePropertyChanged;
				}
			}
		}

		#region IController implementation

		public virtual void Start ()
		{
			App.Current.EventsBroker.Subscribe<LoadTimelineEvent<TModel>> (HandleLoadEvent);
			App.Current.EventsBroker.Subscribe<LoadTimelineEvent<IEnumerable<TModel>>> (HandleLoadEventsList);
			App.Current.EventsBroker.Subscribe<LoadTimelineEvent<EventTypeTimelineVM>> (HandleLoadEventType);
		}

		public virtual void Stop ()
		{
			App.Current.EventsBroker.Unsubscribe<LoadTimelineEvent<TModel>> (HandleLoadEvent);
			App.Current.EventsBroker.Unsubscribe<LoadTimelineEvent<IEnumerable<TModel>>> (HandleLoadEventsList);
			App.Current.EventsBroker.Unsubscribe<LoadTimelineEvent<EventTypeTimelineVM>> (HandleLoadEventType);
		}

		public virtual void SetViewModel (IViewModel viewModel)
		{
			PlayerVM = (VideoPlayerVM)(viewModel as dynamic);
			ViewModel = (TimelineVM)(viewModel as dynamic);
		}

		public virtual IEnumerable<KeyAction> GetDefaultKeyActions ()
		{
			return Enumerable.Empty<KeyAction> ();
		}

		#endregion

		protected virtual void HandlePlayerVMPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
		}

		void HandleLoadEvent (LoadTimelineEvent<TModel> e)
		{
			PlayerVM.LoadEvent (e.Object, e.Playing);
		}

		void HandleLoadEventsList (LoadTimelineEvent<IEnumerable<TModel>> e)
		{
			PlayerVM.LoadEvents (e.Object.OfType<TimelineEvent> (), e.Playing);
		}

		void HandleLoadEventType (LoadTimelineEvent<EventTypeTimelineVM> e)
		{
			var timelineEvents = e.Object.ViewModels.Where ((arg) => arg.Visible == true)
													.Select ((arg) => arg.Model);
			PlayerVM.LoadEvents (timelineEvents, e.Playing);
		}

		void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Collection" || e.PropertyName == "Active") {
				HandleFiltersChanged ();
			}
		}

		void HandleFiltersChanged ()
		{
			foreach (var eventVM in ViewModel.SelectMany (eventTypeVM => eventTypeVM.ViewModels)) {
				eventVM.Visible = ViewModel.Filters.Filter (eventVM);
			}
		}
	}
}

