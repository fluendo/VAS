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
using System.ComponentModel;
using VAS.Core.Common;
using VAS.Core.Events;
using VAS.Core.Interfaces.MVVMC;
using VAS.Core.Services.ViewModel;

namespace VAS.Services.ViewModel
{
	public class RenderingJobsDialogVM : IViewModel
	{
		JobCollectionVM jobCollectionVM;

		public JobCollectionVM JobCollectionVM {
			get {
				return jobCollectionVM;
			}
			set {
				if (jobCollectionVM != null) {
					jobCollectionVM.Selection.CollectionChanged -= OnSelectionChanged;
				}
				jobCollectionVM = value;
				if (jobCollectionVM != null) {
					jobCollectionVM.Selection.CollectionChanged += OnSelectionChanged;
				}
			}
		}

		public bool CancelButtonVisible { get; set; }
		public bool RetryButtonVisible { get; set; }

		public RenderingJobsDialogVM ()
		{
			//JobCollectionVM = new JobCollectionVM ();
			CancelButtonVisible = false;
			RetryButtonVisible = false;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#region commands

		public void ClearDoneJobs ()
		{
			this.PublishNewEvent<ClearDoneJobsEvent> ();
		}

		public void CancelSelectedJobs ()
		{
			this.PublishEvent<CancelSelectedJobsEvent> (
				new CancelSelectedJobsEvent {
					Sender = this,
					Jobs = JobCollectionVM.Selection
				});
		}

		public void RetrySelectedJobs ()
		{
			this.PublishEvent<RetrySelectedJobsEvent> (
				new RetrySelectedJobsEvent {
					Sender = this,
					Jobs = JobCollectionVM.Selection
				});
		}

		#endregion

		protected void OnSelectionChanged (object sender, System.EventArgs e)
		{
			CancelButtonVisible = false;
			RetryButtonVisible = false;

			if (JobCollectionVM.Selection.Count == 0)
				return;

			var jobVM = JobCollectionVM.Selection [0];

			if (jobVM.State == JobState.NotStarted ||
				jobVM.State == JobState.Running)
				CancelButtonVisible = true;

			if (jobVM.State == JobState.Error || jobVM.State == JobState.Cancelled)
				RetryButtonVisible = true;
		}
	}
}
