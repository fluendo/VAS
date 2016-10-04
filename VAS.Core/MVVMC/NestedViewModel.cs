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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using VAS.Core.Interfaces.MVVMC;

namespace VAS.Core.MVVMC
{
	/// <summary>
	/// Base class of a nested ViewModel Type. This type of ViewModel contains an observableCollection of Child
	/// ViewModels. It implements INotifyCollectionChanged and IEnumerable interfaces.
	/// </summary>
	public class NestedViewModel<VMChilds> : BindableBase, INestedViewModel<VMChilds>
		where VMChilds : IViewModel
	{
		public NestedViewModel ()
		{
			ViewModels = new ObservableCollection<VMChilds> ();
		}

		/// <summary>
		/// Gets the collection of child ViewModel
		/// </summary>
		/// <value>The ViewModels collection.</value>
		public ObservableCollection<VMChilds> ViewModels {
			protected set;
			get;
		}

		/// <summary>
		/// Gets the Interface INotifyCollectionChanged of the Child ViewModels
		/// </summary>
		/// <returns>The Collection as a INotifyCollectionChanged</returns>
		public INotifyCollectionChanged GetNotifyCollection ()
		{
			return ViewModels;
		}

		/// <summary>
		/// Gets the enumerator of the Child View Models Collection
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<VMChilds> GetEnumerator ()
		{
			return ViewModels.GetEnumerator ();
		}

		/// <summary>
		/// Gets the enumerator of the Child View Models Collection
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ViewModels.GetEnumerator ();
		}
	}
}

