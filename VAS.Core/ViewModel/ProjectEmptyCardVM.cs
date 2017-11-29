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
using VAS.Core.MVVMC;

namespace VAS.Core.ViewModel
{
	public class ProjectEmptyCardVM : ViewModelBase
	{
		public string HeaderText { get; set; } = Catalog.GetString ("No projects created yet");
		public string DescriptionText { get; set; } = Catalog.GetString ("Tap the + icon to create your first project");
		public string TipText { get; set; } = Catalog.GetString ("Tip: You can get projects from other devices using the sync center");
	}
}
