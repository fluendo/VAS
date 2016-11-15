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
using System.Threading.Tasks;
using VAS.Core.Events;
using VAS.Core.Interfaces.MVVMC;
using VAS.Core.MVVMC;
using VAS.Core.Store;

namespace VAS.Core.ViewModel
{

	/// <summary>
	/// A ViewModel for <see cref="Project"/> objects.
	/// </summary>
	public class ProjectVM : ViewModelBase<Project>
	{
		Project model;

		public ProjectVM ()
		{
			Timers = new CollectionViewModel<Timer, TimerVM> ();
			Playlists = new PlaylistCollectionVM ();
			EventTypes = new CollectionViewModel<EventType, EventTypeVM> ();
			EventTypesTimeline = new EventTypesTimelineVM ();
		}

		public override Project Model {
			get {
				return model;
			}

			set {
				model = value;
				UpdateModels ();
			}
		}

		/// <summary>
		/// Gets the collection of timers in the project.
		/// </summary>
		/// <value>The timers.</value>
		public CollectionViewModel<Timer, TimerVM> Timers {
			get;
			private set;
		}

		/// <summary>
		/// Gets the collection of event types in the project.
		/// </summary>
		/// <value>The event types.</value>
		public CollectionViewModel<EventType, EventTypeVM> EventTypes {
			get;
			private set;
		}

		/// <summary>
		/// Gets collection of playlists in the project.
		/// </summary>
		/// <value>The playlists.</value>
		public PlaylistCollectionVM Playlists {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the event types timeline vm.
		/// </summary>
		/// <value>The event types timeline vm.</value>
		public EventTypesTimelineVM EventTypesTimeline {
			get;
			protected set;
		}

		/// <summary>
		/// Gets the short description of the project.
		/// </summary>
		/// <value>The short description.</value>
		public string ShortDescription {
			get {
				return Model.ShortDescription;
			}
		}

		/// <summary>
		/// Gets or sets the media file set.
		/// </summary>
		/// <value>The file set.</value>
		public MediaFileSet FileSet {
			get {
				return Model.FileSet;
			}
			set {
				Model.FileSet = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the project has been edited.
		/// </summary>
		/// <value><c>true</c> if edited; otherwise, <c>false</c>.</value>
		public bool Edited {
			get {
				return Model?.IsChanged == true;
			}
		}

		protected virtual void UpdateModels ()
		{
			Playlists.Model = Model.Playlists;
			EventTypes.Model = Model.EventTypes;
			Timers.Model = Model.Timers;
		}
	}

	public class ProjectVM<TProject> : ProjectVM, IViewModel<TProject>
		where TProject : Project
	{

		public new TProject Model {
			get {
				return base.Model as TProject;
			}
			set {
				base.Model = value;
			}
		}

		/// <summary>
		/// Command to export a project.
		/// </summary>
		public Task<bool> Export ()
		{
			return App.Current.EventsBroker.PublishWithReturn (new ExportEvent<TProject> { Object = Model });
		}

		/// <summary>
		/// Command to delete a project.
		/// </summary>
		public Task<bool> Delete ()
		{
			return App.Current.EventsBroker.PublishWithReturn (new DeleteEvent<TProject> { Object = Model });
		}

		/// <summary>
		/// Command to save a project.
		/// </summary>
		/// <param name="force">If set to <c>true</c> does not prompt to save.</param>
		public Task<bool> Save (bool force)
		{
			return App.Current.EventsBroker.PublishWithReturn (new UpdateEvent<TProject> { Object = Model, Force = force });
		}
	}
}