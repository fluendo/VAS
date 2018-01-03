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
using System;
using VAS.Core.Common;
using VAS.Core.MVVMC;

namespace VAS.Core.ViewModel
{
	/// <summary>
	/// Dynamic button toolbar ViewModel, this view model exposes a collection of commands to be able to bind it to buttons
	/// </summary>
	public class DynamicButtonToolbarVM : ViewModelBase
    {
		public DynamicButtonToolbarVM ()
		{
			ToolbarCommands = new RangeObservableCollection<Command> ();	
		}

		/// <summary>
		/// Gets or sets the toolbar commands.
		/// </summary>
		/// <value>The toolbar commands.</value>
		public RangeObservableCollection<Command> ToolbarCommands {
			get;
			set;
		}
    }
}
