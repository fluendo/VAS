//
//  Copyright (C) 2015 Andoni Morales Alastruey
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
using Gtk;
using VAS.Core.Common;
using VAS.Core.Events;
using VAS.Core.Interfaces.GUI;

namespace VAS.UI.Helpers
{
	public class ExternalWindow: Window
	{
		EventBox box;

		public ExternalWindow () : base (WindowType.Toplevel)
		{
			// Configure window icon
			Icon = Misc.LoadIcon (App.Current.SoftwareIconName, IconSize.Dialog);

			box = new EventBox ();
			box.Name = "lightbackgroundeventbox";
			box.KeyPressEvent += (o, args) => {
				App.Current.EventsBroker.Publish<KeyPressedEvent> (
					new KeyPressedEvent {
						Sender = this,
						Key = App.Current.Keyboard.ParseEvent (args.Event)
					}
				);
			};
			base.Add (box);
			box.CanFocus = true;
			Focus = box;
			box.Show ();
		}

		public Widget Box {
			get {
				return box;
			}
		}

		new public void Add (Widget widget)
		{
			box.Add (widget);
		}
	}
}

