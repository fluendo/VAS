//
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
using System.Linq;
using VAS.Core.Interfaces.Services;
using VAS.Core.MVVMC;
using VAS.Core.Store;
using VAS.Core.ViewModel;

namespace VAS.Core.Hotkeys
{
	/// <summary>
	/// A service to edit a <see cref="HotKey"/>.
	/// </summary>
	public class HotKeyService : ControllerBase<HotKeyVM>, IHotkeyService
	{
		/// <summary>
		/// Checks if the hotkey is available and change it if possible.
		/// </summary>
		public void SetHotkey (HotKey hotkey, HashSet<HotKey> restrictedHotKeys)
		{
			var newHotKey = App.Current.GUIToolkit.SelectHotkey (hotkey);
			if (restrictedHotKeys.Any (rh => rh == newHotKey)) {
				App.Current.Dialogs.ErrorMessage (Catalog.GetString ("Hotkey already in use: ") + newHotKey, this);
			} else if (newHotKey != null) {
				hotkey.Key = newHotKey.Key;
				hotkey.Modifier = newHotKey.Modifier;
			}
		}
	}
}
