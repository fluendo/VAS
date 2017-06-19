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
using VAS.Core.MVVMC;

namespace VAS.Core.ViewModel.Statistics
{
	/// <summary>
	/// Viewmodel for the horizontal series bar graphic
	/// </summary>
	public class BarChartVM : ChartVM
	{
		/// <summary>
		/// Series collection
		/// </summary>
		/// <value>The series collection.</value>
		public SeriesCollectionVM Series {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating the height of the bars in the collection
		/// </summary>
		/// <value><c>true</c> if height; otherwise, <c>false</c>.</value>
		public double Height { get; set; }
	}
}
