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
using VAS.Core.Store;
using VAS.Core.Store.Templates;

namespace VAS.Core.ViewModel
{
	public class TeamVM : TemplateViewModel<Team, Player, PlayerVM>
	{
		/// <summary>
		/// Gets or sets the icon of the team.
		/// </summary>
		/// <value>The icon.</value>
		public override Image Icon {
			get {
				return Model.Shield;
			}
			set {
				Model.Shield = value;
			}
		}


		public Guid ID {
			get { return Model.ID; }
		}

	}
}
