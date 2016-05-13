﻿//
//  Copyright (C) 2015 Fluendo S.A.
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
using VAS.Core.Store;

namespace VAS.Tests.Core.Store
{
	[TestFixture ()]
	public class TestActionLink
	{

		ActionLink CreateLink ()
		{
			ActionLink link = new ActionLink ();
			link.SourceButton = new DashboardButton ();
			link.SourceTags = new ObservableCollection<Tag> { new Tag ("tag1") };
			link.DestinationButton = new DashboardButton ();
			link.DestinationTags = new ObservableCollection<Tag> { new Tag ("tag2") };
			link.KeepGenericTags = false;
			link.KeepPlayerTags = false;
			return link;
		}

		[Test ()]
		public void TestSerialization ()
		{
			ActionLink link = new ActionLink ();

			Utils.CheckSerialization (link);

			link = CreateLink ();

			ActionLink link2 = Utils.SerializeDeserialize (link);
			Assert.AreEqual (link.SourceTags, link2.SourceTags);
			Assert.AreEqual (link.DestinationTags, link2.DestinationTags);
			Assert.AreEqual (link.KeepGenericTags, link2.KeepGenericTags);
			Assert.AreEqual (link.KeepPlayerTags, link2.KeepPlayerTags);
		}

		[Test ()]
		public void TestEquality ()
		{
			ActionLink link = CreateLink ();
			ActionLink link2 = new ActionLink ();
			Assert.IsTrue (link != link2);
			Assert.AreNotEqual (link, link2);
			link2.SourceButton = link.SourceButton;
			Assert.AreNotEqual (link, link2);
			link2.DestinationButton = link.DestinationButton;
			Assert.AreNotEqual (link, link2);
			link2.SourceTags = new ObservableCollection<Tag> { new Tag ("tag1") }; 
			Assert.AreNotEqual (link, link2);
			link2.DestinationTags = new ObservableCollection<Tag> { new Tag ("tag2") }; 
			Assert.IsTrue (link == link2);
			Assert.IsTrue (link.Equals (link2));
		}

		[Test ()]
		public void TestIsChanged ()
		{
			ActionLink link = CreateLink ();
			Assert.IsTrue (link.IsChanged);
			link.IsChanged = false;
			link.SourceButton = new DashboardButton ();
			Assert.IsTrue (link.IsChanged);
			link.IsChanged = false;
			link.SourceTags.Add (new Tag ("test"));
			Assert.IsTrue (link.IsChanged);
			link.IsChanged = false;
			link.SourceTags = null;
			Assert.IsTrue (link.IsChanged);
			link.IsChanged = false;
			link.DestinationButton = new DashboardButton ();
			Assert.IsTrue (link.IsChanged);
			link.IsChanged = false;
			link.DestinationTags.Remove (link.DestinationTags [0]);
			Assert.IsTrue (link.IsChanged);
			link.IsChanged = false;
			link.DestinationTags = null;
			Assert.IsTrue (link.IsChanged);
			link.IsChanged = false;
			link.KeepPlayerTags = true;
			Assert.IsTrue (link.IsChanged);
			link.IsChanged = false;
			link.KeepGenericTags = true;
			Assert.IsTrue (link.IsChanged);
			link.IsChanged = false;
		}

	}
}

