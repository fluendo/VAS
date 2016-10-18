//
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
using System.ComponentModel;
using Gtk;
using VAS.Core.Hotkeys;
using VAS.Core.Interfaces.GUI;
using VAS.Core.Interfaces.MVVMC;
using VAS.Core.MVVMC;
using VAS.Services.ViewModel;

namespace VAS.UI.Dialog
{
	[ViewAttribute ("RenderingJobs")]
	public partial class RenderingJobsDialog : Gtk.Dialog, IView<RenderingJobsDialogVM>, IPanel
	{
		public RenderingJobsDialog (Window parent)
		{
			TransientFor = parent;
			this.Build ();
			//this.manager = App.Current.RenderingJobsManger;
			cancelbutton.Clicked += OnCancelbuttonClicked;
			clearbutton.Clicked += OnClearbuttonClicked;
			retrybutton.Clicked += OnRetrybuttonClicked;
		}

		public RenderingJobsDialogVM ViewModel {
			get {
				return viewModel;
			}
			set {
				if (ViewModel != null) {
					viewModel.PropertyChanged -= HandleViewModelChanged;
					//ViewModel.JobCollectionVM.Selection.CollectionChanged -= OnSelectionChanged;
				}
				viewModel = value;
				renderingjobstreeview2.SetViewModel (viewModel.JobCollectionVM);
				ViewModel.PropertyChanged += HandleViewModelChanged;
				//ViewModel.JobCollectionVM.Selection.CollectionChanged += OnSelectionChanged;
			}
		}

		RenderingJobsDialogVM viewModel;

		public KeyContext GetKeyContext ()
		{
			KeyContext keyContext = new KeyContext ();
			return keyContext;
		}

		public void OnLoad ()
		{
		}

		public void OnUnload ()
		{
		}

		public void SetViewModel (object viewModel)
		{
			ViewModel = viewModel as RenderingJobsDialogVM;
		}

		public override void Dispose ()
		{
			OnUnload ();
			Destroy ();
			base.Dispose ();
		}

		void HandleViewModelChanged (object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName) {
			case "RetryButtonVisible":
				retrybutton.Visible = ViewModel.RetryButtonVisible;
				retrybutton.QueueDraw ();
				break;

			case "CancelButtonVisible":
				cancelbutton.Visible = ViewModel.RetryButtonVisible;
				cancelbutton.QueueDraw ();
				break;
			}
		}

		protected void OnClearbuttonClicked (object sender, System.EventArgs e)
		{
			ViewModel.ClearDoneJobs ();
		}

		protected void OnCancelbuttonClicked (object sender, System.EventArgs e)
		{
			ViewModel.CancelSelectedJobs ();
		}

		protected void OnRetrybuttonClicked (object sender, System.EventArgs e)
		{
			ViewModel.RetrySelectedJobs ();
		}
	}
}
