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
using System.Collections.ObjectModel;
using NUnit.Framework;
using VAS.Core.Common;
using VAS.Core.Store.Drawables;
using VAS.Core.Store;

namespace VAS.Tests.Core.Store
{
	[TestFixture ()]
	public class TestFrameDrawing
	{
		[Test ()]
		public void TestSerialization ()
		{
			FrameDrawing d = new FrameDrawing ();
			d.Miniature = Utils.LoadImageFromFile ();
			d.Freehand = Utils.LoadImageFromFile ();
			d.Drawables = new ObservableCollection<Drawable> { new Line (), new Rectangle () };
			d.CameraConfig = new CameraConfig (2);
			d.Render = new Time (1000);
			d.Pause = new Time (2000);
			Utils.CheckSerialization (d);

			FrameDrawing d2 = Utils.SerializeDeserialize (d);
			Assert.AreEqual (d.Render, d2.Render);
			Assert.AreEqual (d.Pause, d2.Pause);
			Assert.AreEqual (d.CameraConfig, d2.CameraConfig);
			Assert.AreEqual (d2.Drawables.Count, d.Drawables.Count);
			Assert.IsNotNull (d2.Freehand);
			Assert.IsNotNull (d2.Miniature);
		}

		[Test ()]
		public void TestIsChanged ()
		{
			FrameDrawing d = new FrameDrawing ();
			Assert.IsTrue (d.IsChanged);
			d.IsChanged = false;
			d.CameraConfig = new CameraConfig (1);
			Assert.IsTrue (d.IsChanged);
			d.IsChanged = false;
			d.Freehand = new Image (5, 5);
			Assert.IsTrue (d.IsChanged);
			d.IsChanged = false;
			d.Miniature = new Image (5, 5);
			Assert.IsTrue (d.IsChanged);
			d.IsChanged = false;
			d.Pause = new Time (5);
			Assert.IsTrue (d.IsChanged);
			d.IsChanged = false;
			d.RegionOfInterest = new Area (23, 23, 23, 23);
			Assert.IsTrue (d.IsChanged);
			d.IsChanged = false;
			d.Render = new Time (2);
			Assert.IsTrue (d.IsChanged);
			d.IsChanged = false;
			d.Drawables.Add (new Line ());
			Assert.IsTrue (d.IsChanged);
			d.IsChanged = false;
			d.Drawables = null;
			Assert.IsTrue (d.IsChanged);
			d.IsChanged = false;
		}
	}
}

