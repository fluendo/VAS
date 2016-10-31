// 
//  Copyright (C) 2011 Andoni Morales Alastruey
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
using VAS.Core.Common;
using VAS.Core.Services.ViewModel;

namespace VAS.Core.Interfaces
{
	public interface IRenderingJobsController
	{
		void RetryJobs (RangeObservableCollection<JobVM> retryJobs);

		void DeleteJob (JobVM job);

		void ClearDoneJobs ();

		void CancelJobs (RangeObservableCollection<JobVM> cancelJobs);

		void CancelCurrentJob ();

		void CancelJob (JobVM job);

		void CancelAllJobs ();

		void AddJob (Job job);

		JobCollectionVM Jobs { get; }

		JobCollectionVM PendingJobs { get; }
	}
}

