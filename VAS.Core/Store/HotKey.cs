// HotKey.cs
//
//  Copyright (C) 2007-2009 Andoni Morales Alastruey
//
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
//
using System;
using Newtonsoft.Json;
using VAS.Core;
using VAS.Core.Common;
using VAS.Core.Interfaces;
using VAS.Core.MVVMC;

namespace VAS.Core.Store
{
	/// <summary>
	/// A key combination used to tag plays using the keyboard. <see cref="LongoMatch.Store.SectionsTimeNodes"/>
	/// It can only be used with the Shith and Alt modifiers to avoid interfering with ohter shortcuts.
	/// 'key' and 'modifier' are set to -1 when it's initialized
	/// </summary>
	[Serializable]
	[PropertyChanged.ImplementPropertyChanged]
	public class HotKey : BindableBase, IEquatable<HotKey>
	{
		#region Constructors

		/// <summary>
		/// Creates a new undefined HotKey
		/// </summary>
		public HotKey ()
		{
			Key = -1;
			Modifier = -1;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gdk Key
		/// </summary>
		public int Key {
			get;
			set;
		}

		/// <summary>
		/// Key modifier. Only Alt and Shift can be used
		/// </summary>
		public int Modifier {
			get;
			set;
		}

		/// <summary>
		/// Get whether the hotkey is defined or not
		/// </summary>
		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public Boolean Defined {
			get {
				return (Key != -1);
			}
		}

		#endregion

		#region Public Methods

		public bool Equals (HotKey hotkeyComp)
		{
			if (hotkeyComp == null)
				return false;
			return (this.Key == hotkeyComp.Key && this.Modifier == hotkeyComp.Modifier);
		}

		#endregion

		#region Operators

		static public bool operator == (HotKey a, HotKey b)
		{
			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals (a, b)) {
				return true;
			}

			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null)) {
				return false;
			}
			return a.Equals (b);
		}

		static public bool operator != (HotKey a, HotKey b)
		{
			return !(a == b);
		}

		#endregion

		#region Overrides

		public override bool Equals (object obj)
		{
			if (obj is HotKey) {
				HotKey hotkey = obj as HotKey;
				return Equals (hotkey);
			} else
				return false;
		}

		public override int GetHashCode ()
		{
			return Key ^ Modifier;
		}

		public override string ToString ()
		{
			if (!Defined)
				return Catalog.GetString ("Not defined");
			return Keyboard.HotKeyName (this);
		}

		#endregion
	}
}
