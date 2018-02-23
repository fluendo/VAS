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
using System;
using VAS.Core.Common;

namespace VAS.Core.Store.Drawables
{
	/// <summary>
	/// A rectangular area, identical to a rectangle but without borders.
	/// </summary>
	[Serializable]
	public class RectangleArea : Rectangle
	{

		public RectangleArea (Point center, double axisX, double axisY) : base (center, axisX, axisY) { }

		public override int LineWidth { get => 0; }

		public override Color StrokeColor { get => Color.Transparent; }

	}
}
