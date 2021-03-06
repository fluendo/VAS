﻿//
//  Copyright (C) 2014 Andoni Morales Alastruey
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

namespace VAS.Core.Store
{
	[Serializable]
	public class TimedDashboardButton : DashboardButton
	{
		public TimedDashboardButton ()
		{
			TagMode = TagMode.Predefined;
			Start = new Time { TotalSeconds = 10 };
			if (Start != null) {
				Start.IsChanged = false;
			}
			Stop = new Time { TotalSeconds = 10 };
			if (Stop != null) {
				Stop.IsChanged = false;
			}
		}

		public TagMode TagMode {
			get;
			set;
		}

		public virtual Time Start {
			get;
			set;
		}

		public virtual Time Stop {
			get;
			set;
		}
	}
}

