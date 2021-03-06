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

namespace VAS.Core.MVVMC
{
	/// <summary>
	/// Attribute used to register IView components.
	/// </summary>
	[AttributeUsage (AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ViewAttribute : Attribute
	{
		public ViewAttribute (string viewName, int priority = 0)
		{
			ViewName = viewName;
			Priority = priority;
		}

		/// <summary>
		/// The name of the View.
		/// </summary>
		/// <value>The name of the view.</value>
		public string ViewName {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the priority used to register this view.
		/// The higher the more priority.
		/// </summary>
		/// <value>The priority.</value>
		public int Priority {
			get;
			set;
		}
	}
}

