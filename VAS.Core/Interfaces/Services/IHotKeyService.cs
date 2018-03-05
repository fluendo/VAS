﻿//
//  Copyright (C) 2018 Fluendo S.A.
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
using System.Collections.Generic;
using VAS.Core.Store;

namespace VAS.Core.Interfaces.Services
{
	/// <summary>
	/// This service provides methods for managing a single hotkey.
	/// </summary>
	public interface IHotkeyService
	{
		/// <summary>
		/// Checks if the hotkey is available and change it if possible.
		/// </summary>
		void SetHotkey (HotKey hotkey, HashSet<HotKey> restrictedHotKeys);
	}
}
