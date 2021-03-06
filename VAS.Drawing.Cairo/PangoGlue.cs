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
using System.Runtime.InteropServices;

namespace VAS.Drawing.Cairo
{
	public static class PangoGlue
	{
		[DllImport ("libpango-1.0.dll")]
		static extern void pango_layout_set_height (IntPtr layout, int height);

		public static void SetPangoLayoutHeight (this Pango.Layout layout, int height)
		{
			pango_layout_set_height (layout.Handle, height);
		}
	}
}