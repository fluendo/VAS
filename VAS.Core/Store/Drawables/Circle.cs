//
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
using Newtonsoft.Json;
using VAS.Core.Common;

namespace VAS.Core.Store.Drawables
{
	[Serializable]
	public class Circle: Ellipse
	{
		public Circle ()
		{
		}

		public Circle (Point center, double radius) :
			base (center, radius, radius)
		{
		}

		public double Radius {
			get;
			set;
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public override double AxisY {
			get {
				return Radius;
			}
			set {
				Radius = value;
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public override double AxisX {
			get {
				return Radius;
			}
			set {
				Radius = value;
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public override Area Area {
			get {
				return new Area (new Point (Center.X - Radius, Center.Y - Radius),
					Radius * 2, Radius * 2);
			}
		}
	}
}

