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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using VAS.Core;
using VAS.Core.Common;
using VAS.Core.Events;
using VAS.Core.Hotkeys;
using VAS.Core.Interfaces;
using VAS.Core.Interfaces.MVVMC;
using VAS.Core.MVVMC;
using VAS.Core.Serialization;
using VAS.Core.ViewModel;
using VAS.Services.ViewModel;

namespace VAS.Services.Controller
{

	/// <summary>
	/// Base Controller for working with <see cref="ITemplate"/> like dashboards and teams.
	/// </summary>
	public abstract class TemplatesController<TModel, TViewModel> : DisposableBase, IController
		where TModel : BindableBase, ITemplate<TModel>, new()
		where TViewModel : TemplateViewModel<TModel>, new()
	{
		TemplatesManagerViewModel<TModel, TViewModel> viewModel;
		ITemplateProvider<TModel> provider;
		bool started;

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			Stop ();
		}

		public TemplatesManagerViewModel<TModel, TViewModel> ViewModel {
			get {
				return viewModel;
			}
			set {
				if (viewModel != null) {
					viewModel.PropertyChanged -= HandleSelectionChanged;
					viewModel.LoadedTemplate.PropertyChanged -= HandleTemplateChanged;
				}
				viewModel = value;
				viewModel.PropertyChanged += HandleSelectionChanged;
				viewModel.LoadedTemplate.PropertyChanged += HandleTemplateChanged;
				if (viewModel.Selection.Count == 0) {
					viewModel.Select (viewModel.Model.FirstOrDefault ());
				}
			}
		}

		protected string FilterText { get; set; }

		protected string AlreadyExistsText { get; set; }

		protected string ExportedCorrectlyText { get; set; }

		protected string CountText { get; set; }

		protected string NewText { get; set; }

		protected string OverwriteText { get; set; }

		protected string ErrorSavingText { get; set; }

		protected string ConfirmDeleteText { get; set; }

		protected string ConfirmDeleteListText { get; set; }

		protected string CouldNotLoadText { get; set; }

		protected string NotEditableText { get; set; }

		protected string ConfirmSaveText { get; set; }

		protected string ImportText { get; set; }

		protected string NameText { get; set; }

		protected string TemplateName { get; set; }

		protected string Extension { get; set; }

		/// <summary>
		/// Gets or sets the name of the transition used in HandleOpen
		/// </summary>
		/// <value>The name of the open transition.</value>
		protected string OpenTransitionName { get; set; }

		protected ITemplateProvider<TModel> Provider {
			get {
				return provider;
			}
			set {
				provider = value;
			}
		}

		#region IController implementation

		public virtual void SetViewModel (IViewModel viewModel)
		{
			ViewModel = (TemplatesManagerViewModel<TModel, TViewModel>)(viewModel as dynamic);
		}

		public virtual void Start ()
		{
			if (started) {
				return;
			}
			provider.CollectionChanged += HandleProviderCollectionChanged;
			App.Current.EventsBroker.SubscribeAsync<ExportEvent<TModel>> (HandleExport);
			App.Current.EventsBroker.SubscribeAsync<ImportEvent<TModel>> (HandleImport);
			App.Current.EventsBroker.SubscribeAsync<UpdateEvent<TModel>> (HandleSave);
			App.Current.EventsBroker.SubscribeAsync<OpenEvent<TModel>> (HandleOpen);
			App.Current.EventsBroker.SubscribeAsync<CreateEvent<TModel>> (HandleNew);
			App.Current.EventsBroker.SubscribeAsync<ChangeNameEvent<TModel>> (HandleChangeName);
			App.Current.EventsBroker.SubscribeAsync<DeleteEvent<ObservableCollection<TModel>>> (HandleDelete);
			started = true;
		}

		public virtual void Stop ()
		{
			if (!started) {
				return;
			}
			provider.CollectionChanged -= HandleProviderCollectionChanged;
			App.Current.EventsBroker.UnsubscribeAsync<ExportEvent<TModel>> (HandleExport);
			App.Current.EventsBroker.UnsubscribeAsync<ImportEvent<TModel>> (HandleImport);
			App.Current.EventsBroker.UnsubscribeAsync<UpdateEvent<TModel>> (HandleSave);
			App.Current.EventsBroker.UnsubscribeAsync<OpenEvent<TModel>> (HandleOpen);
			App.Current.EventsBroker.UnsubscribeAsync<CreateEvent<TModel>> (HandleNew);
			App.Current.EventsBroker.UnsubscribeAsync<ChangeNameEvent<TModel>> (HandleChangeName);
			App.Current.EventsBroker.UnsubscribeAsync<DeleteEvent<ObservableCollection<TModel>>> (HandleDelete);
			started = false;
		}

		public IEnumerable<KeyAction> GetDefaultKeyActions ()
		{
			return Enumerable.Empty<KeyAction> ();
		}

		#endregion

		protected abstract bool SaveValidations (TModel model);

		#region Handle Events

		async Task HandleExport (ExportEvent<TModel> evt)
		{
			string fileName, filterName;
			string [] extensions;

			TModel template = evt.Object;
			Log.Debug ("Exporting " + TemplateName);
			filterName = FilterText;
			extensions = new [] { "*" + Extension };
			/* Show a file chooser dialog to select the file to export */
			fileName = App.Current.Dialogs.SaveFile (Catalog.GetString ("Export dashboard"),
				System.IO.Path.ChangeExtension (template.Name, Extension),
				App.Current.HomeDir, filterName, extensions);

			if (fileName != null) {
				fileName = System.IO.Path.ChangeExtension (fileName, Extension);
				if (System.IO.File.Exists (fileName)) {
					string msg = AlreadyExistsText + " " + OverwriteText;
					evt.ReturnValue = await App.Current.Dialogs.QuestionMessage (msg, null);
				}

				if (evt.ReturnValue) {
					Serializer.Instance.Save (template, fileName);
					App.Current.Dialogs.InfoMessage (ExportedCorrectlyText);
				}
			}
		}

		async Task HandleImport (ImportEvent<TModel> evt)
		{
			string fileName, filterName;
			string [] extensions;

			TModel template = evt.Object;
			Log.Debug ("Importing dashboard");
			filterName = Catalog.GetString (FilterText);
			extensions = new [] { "*" + Extension };
			/* Show a file chooser dialog to select the file to import */
			fileName = App.Current.Dialogs.OpenFile (ImportText, null, App.Current.HomeDir,
				filterName, extensions);

			if (fileName == null)
				return;

			try {
				TModel newTemplate = Provider.LoadFile (fileName);

				if (newTemplate != null) {
					bool abort = false;

					while (Provider.Exists (newTemplate.Name) && !abort) {
						string name = await App.Current.Dialogs.QueryMessage (NameText,
										  Catalog.GetString ("Name conflict"), newTemplate.Name + "#");
						if (name == null) {
							abort = true;
						} else {
							newTemplate.Name = name;
						}
					}

					if (!abort) {
						Provider.Save (newTemplate);
						ViewModel.Select (newTemplate);
						evt.ReturnValue = true;
					}
				}
			} catch (Exception ex) {
				App.Current.Dialogs.ErrorMessage (Catalog.GetString ("Error importing file:") +
				"\n" + ex.Message);
				Log.Exception (ex);
			}
		}

		async protected virtual Task HandleOpen (OpenEvent<TModel> evt)
		{
			dynamic properties = new ExpandoObject ();
			properties.Object = evt.Object.Clone ();
			await App.Current.StateController.MoveToModal (OpenTransitionName, properties);
		}

		async protected virtual Task HandleNew (CreateEvent<TModel> evt)
		{
			TModel template, templateToDelete;

			if (ViewModel.LoadedTemplate.Edited) {
				await HandleSave (new UpdateEvent<TModel> { Force = false, Object = ViewModel.LoadedTemplate.Model });
			}

			if (!await App.Current.GUIToolkit.CreateNewTemplate<TModel> (ViewModel.Model.ToList (),
					NewText, CountText, Catalog.GetString ("The name is empty."), evt)) {
				return;
			}

			templateToDelete = Provider.Templates.FirstOrDefault (t => t.Name == evt.Name);
			if (templateToDelete != null) {
				var msg = AlreadyExistsText + " " + OverwriteText;
				if (!await App.Current.Dialogs.QuestionMessage (msg, null, msg)) {
					return;
				}
			}

			if (evt.Source != null) {
				try {
					template = Provider.Copy (evt.Source, evt.Name);
				} catch (InvalidTemplateFilenameException ex) {
					App.Current.Dialogs.ErrorMessage (ex.Message, this);
					return;
				}
			} else {
				template = Provider.Create (evt.Name, evt.Count);
				if (!SaveTemplate (template)) {
					App.Current.Dialogs.ErrorMessage (ErrorSavingText);
					return;
				}
			}
			if (templateToDelete != null) {
				Provider.Delete (templateToDelete);
			}
			ViewModel.Select (template);
			evt.ReturnValue = true;
		}

		async protected virtual Task HandleDelete (DeleteEvent<ObservableCollection<TModel>> evt)
		{
			ObservableCollection<TModel> templates = evt.Object;

			if (templates != null) {
				string msg = templates.Count () == 1 ?
							String.Format (ConfirmDeleteText, templates.FirstOrDefault ().Name) : ConfirmDeleteListText;
				if (await App.Current.Dialogs.QuestionMessage (msg, null)) {
					foreach (TModel template in templates) {
						Provider.Delete (template);
					}
					viewModel.Select (viewModel.Model.FirstOrDefault ());
					evt.ReturnValue = true;
				}
			}
		}

		async Task HandleSave (UpdateEvent<TModel> evt)
		{
			TModel template = evt.Object;
			bool force = evt.Force;
			TViewModel templateViewmodel = ViewModel.ViewModels.FirstOrDefault (vm => vm.Model.Equals (template));

			if (template == null || !template.IsChanged || !SaveValidations (template)) {
				return;
			}

			if (template.Static) {
				/* prompt=false when we click the save button */
				if (force) {
					evt.ReturnValue = await SaveStatic (template);
				}
			} else {
				string msg = ConfirmSaveText;
				if (force || await App.Current.Dialogs.QuestionMessage (msg, null, this)) {
					evt.ReturnValue = SaveTemplate (template);
					if (evt.ReturnValue) {
						if (templateViewmodel == null) {
							// When is a new template, we should get the VM again, because previously was null
							templateViewmodel = ViewModel.ViewModels.FirstOrDefault (vm => vm.Model.Equals (template));
						} else {
							// Update the ViewModel with the model clone used for editting. If it was a new one isn't necessary
							templateViewmodel.Model = template;
						}
						ViewModel.SaveSensitive = false;
					}
				}
			}
		}

		protected virtual async void HandleSelectionChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != "Selection") {
				return;
			}

			TViewModel selectedVM = ViewModel.Selection.FirstOrDefault ();
			TModel loadedTemplate = default (TModel);

			if (ViewModel.LoadedTemplate.Edited == true) {
				await HandleSave (new UpdateEvent<TModel> { Force = false, Object = ViewModel.LoadedTemplate.Model });
			}

			if (selectedVM != null) {
				TModel template = selectedVM.Model;
				try {
					// Create a clone of the template and set it in the DashboardViewModel to edit
					// changes in a different model.
					loadedTemplate = template.Clone (SerializationType.Json);
					loadedTemplate.IsChanged = false;
					loadedTemplate.Static = template.Static;
				} catch (Exception ex) {
					Log.Exception (ex);
					App.Current.Dialogs.ErrorMessage (CouldNotLoadText);
					return;
				}
			}
			// Load the model
			ViewModel.LoadedTemplate.Model = loadedTemplate;
			// Update controls visiblity
			ViewModel.DeleteSensitive = loadedTemplate != null && ViewModel.LoadedTemplate.Editable;
			ViewModel.ExportSensitive = loadedTemplate != null;
			ViewModel.SaveSensitive = false;

			//Update commands
			ViewModel.DeleteCommand.EmitCanExecuteChanged ();
		}

		async Task HandleChangeName (ChangeNameEvent<TModel> evt)
		{
			TModel template = evt.Object;
			string newName = evt.NewName;

			if (String.IsNullOrEmpty (newName)) {
				App.Current.Dialogs.ErrorMessage (Catalog.GetString ("The name is empty."));
				return;
			}
			if (template.Name == newName) {
				return;
			}
			if (Provider.Exists (newName)) {
				App.Current.Dialogs.ErrorMessage (AlreadyExistsText, this);
			} else {
				template.Name = newName;
				Provider.Save (template);
				evt.ReturnValue = true;
			}
			await AsyncHelpers.Return ();
		}

		void HandleTemplateChanged (object sender, PropertyChangedEventArgs e)
		{
			ViewModel.SaveSensitive = true;
		}


		void HandleProviderCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action) {
			case NotifyCollectionChangedAction.Add:
				foreach (TModel template in e.NewItems)
					ViewModel.Model.Add (template);
				break;
			case NotifyCollectionChangedAction.Remove:
				foreach (TModel template in e.OldItems)
					ViewModel.Model.Remove (template);
				break;
			case NotifyCollectionChangedAction.Replace:
				break;
			}
		}

#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
		#endregion

		async Task<bool> SaveStatic (TModel template)
		{
			string msg = NotEditableText;
			bool saveOk = false;
			if (await App.Current.Dialogs.QuestionMessage (msg, null, this)) {
				string newName;
				while (true) {
					newName = await App.Current.Dialogs.QueryMessage (Catalog.GetString ("Name:"), null,
						template.Name + "_copy", this);
					if (newName == null)
						break;
					if (Provider.Exists (newName)) {
						msg = AlreadyExistsText;
						App.Current.Dialogs.ErrorMessage (msg, this);
					} else {
						break;
					}
				}
				if (newName == null) {
					return false;
				}
				TModel newtemplate = template.Copy (newName);
				newtemplate.Static = false;
				saveOk = SaveTemplate (newtemplate);
			}

			return saveOk;
		}

		bool SaveTemplate (TModel template)
		{
			try {
				Provider.Save (template);
				return true;
			} catch (InvalidTemplateFilenameException ex) {
				App.Current.Dialogs.ErrorMessage (ex.Message, this);
				return false;
			}
		}
	}
}
