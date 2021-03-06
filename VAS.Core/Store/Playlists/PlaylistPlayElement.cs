//
//  Copyright (C) 2014 Andoni Morales Alastruey
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
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using VAS.Core.Common;
using VAS.Core.Interfaces;
using VAS.Core.MVVMC;

namespace VAS.Core.Store.Playlists
{
	[Serializable]
	public class PlaylistPlayElement : BindableBase, IPlaylistEventElement
	{
		public PlaylistPlayElement (TimelineEvent play)
		{
			Play = play;
			Title = play.Name;
			Rate = play.Rate;
			CamerasLayout = play.CamerasLayout;
			CamerasConfig = new RangeObservableCollection<CameraConfig> (play.CamerasConfig);
		}

		protected override void DisposeManagedResources ()
		{
			base.DisposeManagedResources ();
			Play?.Dispose ();
			Play = null;
			CamerasConfig.Clear ();
		}

		/// <summary>
		/// The event associated to this playlist element
		/// </summary>
		public TimelineEvent Play {
			get;
			set;
		}

		[JsonIgnore]
		public Time Start {
			get {
				return Play.Start;
			}
			set {
				Play.Start = value;
			}
		}

		[JsonIgnore]
		public Time Stop {
			get {
				return Play.Stop;
			}
			set {
				Play.Stop = value;
			}
		}

		/// <summary>
		/// The duration of the playlist element
		/// </summary>
		[JsonIgnore]
		public Time Duration {
			get {
				return Play.Duration;
			}
		}

		/// <summary>
		/// Gets or sets the title of the playlist element
		/// </summary>
		/// <value>The title of the playlist element</value>
		public string Title {
			get;
			set;
		}

		/// <summary>
		/// Override the default <see cref="TimelineEvent.Rate"/>
		/// defined by the <see cref="TimelineEvent"/>
		/// </summary>
		public float Rate {
			get;
			set;
		}

		/// <summary>
		/// A string representing this playback rate
		/// </summary>
		public string RateString {
			get {
				return String.Format ("{0}X", Rate);
			}
		}

		/// <summary>
		/// Override the default <see cref="TimelineEvent.CamerasLayout"/>
		/// defined by the <see cref="TimelineEvent"/>
		/// </summary>
		public object CamerasLayout {
			get;
			set;
		}

		/// <summary>
		/// Override the default <see cref="TimelineEvent.CamerasConfig"/>
		/// defined by the <see cref="TimelineEvent"/>
		/// </summary>
		public RangeObservableCollection<CameraConfig> CamerasConfig {
			get;
			set;
		}

		[JsonIgnore]
		public RangeObservableCollection<FrameDrawing> Drawings {
			get {
				return Play.Drawings;
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public string Description {
			get {
				return (Title + "\n" + TagsDescription () + "\n" + TimesDesription ());
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public Image Miniature {
			get {
				return Play.Miniature;
			}
		}

		public override string ToString ()
		{
			if (CamerasConfig.Count > 0) {
				return string.Format ("Duration={0}, Title={1}, Rate={2}, ROI={3}, Drawings={4}",
					Duration, Title, RateString, CamerasConfig [0].RegionOfInterest,
					Play.Drawings.Count);
			} else {
				return string.Format ("Duration={0}, Title={1}, Rate={2}, ROI=None, Drawings={3}",
					Duration, Title, RateString,
					Play.Drawings.Count);
			}

		}

		string TagsDescription ()
		{
			return String.Join ("-", Play?.Tags?.Select (t => t.Value));
		}

		string TimesDesription ()
		{
			var desc = Play?.Start?.ToMSecondsString () + " - " + Play?.Stop?.ToMSecondsString ();
			if (Rate != 1) {
				desc += " (" + RateString + ")";
			}
			return desc;
		}
	}
}

