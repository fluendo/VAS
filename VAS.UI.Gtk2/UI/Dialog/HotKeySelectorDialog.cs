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
using Gdk;
using Gtk;
using VAS.Core.Store;

namespace VAS.UI.Dialog
{
	public partial class HotKeySelectorDialog : Gtk.Dialog
	{
		HotKey hotKey;

		#region Constructors

		public HotKeySelectorDialog (Gtk.Window parent)
		{
			TransientFor = parent;
			hotKey = new HotKey ();
			this.Build ();
		}

		#endregion

		#region Properties

		public HotKey HotKey {
			get {
				return this.hotKey;
			}
		}

		#endregion

		#region Overrides

		protected override bool OnKeyPressEvent (Gdk.EventKey evnt)
		{
			if (evnt.Key == Gdk.Key.Escape || evnt.Key == Gdk.Key.Return) {
				return base.OnKeyPressEvent (evnt);
			}

			if (IsSupportedModifier (evnt.Key)) {
				return true;
			}

			hotKey = App.Current.Keyboard.ParseEvent (evnt);
			Respond (ResponseType.Ok);
			return true;
		}

		#endregion

		bool IsSupportedModifier (Gdk.Key key)
		{
			return key == Gdk.Key.Shift_L ||
			key == Gdk.Key.Shift_R ||
			key == Gdk.Key.Alt_L ||
			key == Gdk.Key.Alt_R ||
			key == Gdk.Key.Control_L ||
			key == Gdk.Key.Control_R ||
			key == (Gdk.Key)ModifierType.None;
		}

	}
}
