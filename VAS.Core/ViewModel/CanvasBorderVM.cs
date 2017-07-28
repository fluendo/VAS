﻿//
//  Copyright (C) 2017 FLUENDO S.A.
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
using VAS.Core.Interfaces.Drawing;
using VAS.Core.MVVMC;

namespace VAS.Core.ViewModel
{
	/// <summary>
	/// Viewmodel of canvas surrounded by a border
	/// </summary>
	public class CanvasBorderVM : ViewModelBase
	{
		protected override void DisposeManagedResources ()
		{
			base.DisposeManagedResources ();
			CanvasObject.Dispose ();
			CanvasObject = null;
			BorderVM.Dispose ();
			BorderVM = null;
		}
		/// <summary>
		/// Gets or sets the canvas object.
		/// </summary>
		/// <value>The canvas object.</value>
		public ICanvasObject CanvasObject { get; set; }

		/// <summary>
		/// Gets or sets the border vm.
		/// </summary>
		/// <value>The border vm.</value>
		public BorderRegionVM BorderVM { get; set; }
	}
}
