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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using VAS.Core;
using VAS.Core.Common;
using VAS.Core.Events;
using VAS.Core.Hotkeys;
using VAS.Core.Interfaces.GUI;
using VAS.Core.Interfaces.MVVMC;
using VAS.Core.Serialization;
using VAS.Core.Store;
using VAS.Services.ViewModel;

namespace VAS.Services.Controller
{
	public class ProjectsController<T, W> : IController where T : Project where W : ProjectVM<T>, new()
	{
		bool started;
		ProjectsManagerVM<T, W> viewModel;

		ProjectsManagerVM<T, W> ViewModel {
			get {
				return viewModel;
			}
			set {
				viewModel = value;
				viewModel.PropertyChanged += HandleSelectionChanged;
				viewModel.Select (viewModel.Model.FirstOrDefault ());
			}
		}

		#region IController implementation

		public void SetViewModel (IViewModel viewModel)
		{
			ViewModel = (ProjectsManagerVM<T, W>)viewModel;
		}

		public virtual void Start ()
		{
			if (started) {
				throw new InvalidOperationException ("The controller is already running");
			}
			App.Current.EventsBroker.Subscribe<ExportEvent<T>> (HandleExport);
			App.Current.EventsBroker.Subscribe<ImportEvent<T>> (HandleImport);
			App.Current.EventsBroker.Subscribe<UpdateEvent<T>> (HandleSave);
			App.Current.EventsBroker.Subscribe<CreateEvent<T>> (HandleNew);
			App.Current.EventsBroker.Subscribe<DeleteEvent<T>> (HandleDelete);
			started = true;
		}

		public virtual void Stop ()
		{
			if (!started) {
				throw new InvalidOperationException ("The controller is already stopped");
			}
			App.Current.EventsBroker.Unsubscribe<ExportEvent<T>> (HandleExport);
			App.Current.EventsBroker.Unsubscribe<ImportEvent<T>> (HandleImport);
			App.Current.EventsBroker.Unsubscribe<UpdateEvent<T>> (HandleSave);
			App.Current.EventsBroker.Unsubscribe<CreateEvent<T>> (HandleNew);
			App.Current.EventsBroker.Unsubscribe<DeleteEvent<T>> (HandleDelete);
			started = false;
		}

		public virtual IEnumerable<KeyAction> GetDefaultKeyActions ()
		{
			return Enumerable.Empty<KeyAction> ();
		}

		#endregion

		void HandleExport (ExportEvent<T> evt)
		{
			// Only handle regular exports from here
			if (evt.Format != null || evt.Object == null) {
				return;
			}
			Project project = evt.Object;
			string projectExt = App.Current.ProjectExtension;

			string filename = App.Current.Dialogs.SaveFile (
				Catalog.GetString ("Export project"),
				Utils.SanitizePath (project.ShortDescription + projectExt),
				App.Current.HomeDir, projectExt,
				new string [] { projectExt });
			if (filename != null) {
				filename = System.IO.Path.ChangeExtension (filename, projectExt);
				Serializer.Instance.Save (project, filename);
			}
		}

		void HandleImport (ImportEvent<T> evt)
		{
		}

		void HandleNew (CreateEvent<T> evt)
		{
		}

		async void HandleDelete (DeleteEvent<T> evt)
		{
			T project = evt.Object;

			if (project == null) {
				return;
			}

			string msg = Catalog.GetString ("Do you really want to delete:") + "\n" + project.ShortDescription;
			if (await App.Current.Dialogs.QuestionMessage (msg, null)) {
				IBusyDialog busy = App.Current.Dialogs.BusyDialog (Catalog.GetString ("Deleting project..."), null);
				busy.ShowSync (() => {
					try {
						App.Current.DatabaseManager.ActiveDB.Delete<T> (project);
						ViewModel.Model.Remove (project);
						viewModel.Select (viewModel.Model.FirstOrDefault ());
					} catch (StorageException ex) {
						App.Current.Dialogs.ErrorMessage (ex.Message);
					}
				});
			}
		}

		async void HandleSave (UpdateEvent<T> evt)
		{
			T project = evt.Object;
			if (project == null) {
				return;
			}
			await Save (project, true);
		}

		async void HandleSelectionChanged (object sender, PropertyChangedEventArgs e)
		{
			T loadedProject = null;

			if (e.PropertyName != "Selection") {
				return;
			}

			ProjectVM<T> projectVM = ViewModel.Selection.FirstOrDefault ();

			if (ViewModel.LoadedProject.Edited == true) {
				await Save (ViewModel.LoadedProject.Model, false);
			}

			// Load the model, creating a copy of the Project to edit changes in a different model in case the user
			// does not want to save them.
			T project = projectVM.Model;
			loadedProject = project.Clone (SerializationType.Json);
			project.IsChanged = false;
			ViewModel.LoadedProject.Model = loadedProject;

			// Update controls visiblity
			ViewModel.DeleteSensitive = loadedProject != null;
			ViewModel.ExportSensitive = loadedProject != null;
			ViewModel.SaveSensitive = false;
		}

		async Task Save (T project, bool force)
		{
			if (!force && project.IsChanged) {
				string msg = Catalog.GetString ("Do you want to save the current project?");
				if (!(await App.Current.Dialogs.QuestionMessage (msg, null, this))) {
					return;
				}
			}
			try {
				IBusyDialog busy = App.Current.Dialogs.BusyDialog (Catalog.GetString ("Saving project..."), null);
				busy.ShowSync (() => App.Current.DatabaseManager.ActiveDB.Store<T> (project));
				// Update the ViewModel with the model clone used for editting.
				ViewModel.ViewModels.FirstOrDefault (vm => vm.Model.Equals (project)).Model = project;
				ViewModel.SaveSensitive = false;
			} catch (Exception ex) {
				Log.Exception (ex);
				App.Current.Dialogs.ErrorMessage (Catalog.GetString ("Error saving project:") + "\n" + ex.Message);
				return;
			}
		}
	}
}
