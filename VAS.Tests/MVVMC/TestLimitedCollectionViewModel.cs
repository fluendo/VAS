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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using VAS.Core.License;
using VAS.Core.MVVMC;
using VAS.Core.ViewModel;

namespace VAS.Tests.MVVMC
{
	[TestFixture]
	public class TestLimitedCollectionViewModel
	{
		LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> col;
		List<Utils.PlayerDummy> players;

		[SetUp]
		public void SetUp ()
		{
			col = new LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> ();
			col.Limitation = new LicenseLimitationVM ();
			players = CreateDummyPlayers ();
		}

		[Test]
		public void TestWithoutLimitationWithoutSort ()
		{
			col.SortByCreationDateDesc = false;

			col.Model.AddRange (players);

			CheckPlayersInLimitedCollectionBy (new int [] { 1, 2, 0 }, players, col);
		}

		[Test]
		public void TestWithoutLimitationWithSort ()
		{
			col.Model.AddRange (players);

			CheckPlayersInLimitedCollectionBy (new int [] { 0, 2, 1 }, players, col);
		}

		[Test]
		public void TestWithLimitationDisabledWithoutSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = false, Maximum = 2 };
			col.SortByCreationDateDesc = false;

			col.Limitation.Model = ll;
			col.Model.AddRange (players);

			CheckPlayersInLimitedCollectionBy (new int [] { 1, 2, 0 }, players, col);
		}

		[Test]
		public void TestWithLimitationDisabledWithSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = false, Maximum = 2 };

			col.Limitation.Model = ll;
			col.Model.AddRange (players);

			CheckPlayersInLimitedCollectionBy (new int [] { 0, 2, 1 }, players, col);
		}

		[Test]
		public void TestWithLimitationEnabledWithoutSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = true, Maximum = 2 };
			col.SortByCreationDateDesc = false;

			col.Limitation.Model = ll;
			col.Model.AddRange (players);

			CheckPlayersInLimitedCollectionBy (new int [] { 1, 2 }, players, col);
		}

		[Test]
		public void TestWithLimitationEnabledWithSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = true, Maximum = 2 };

			col.Limitation.Model = ll;
			col.Model.AddRange (players);

			CheckPlayersInLimitedCollectionBy (new int [] { 0, 2 }, players, col);
		}

		[Test]
		public void TestAddWithLimitationEnabledWithSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = true, Maximum = 3 };

			col.Limitation.Model = ll;
			col.Model.AddRange (players);
			List<Utils.PlayerDummy> newPlayers = new List<Utils.PlayerDummy> ();
			newPlayers.Add (new Utils.PlayerDummy { CreationDate = DateTime.Now.AddMinutes (1), Name = "P4" });
			players.AddRange (newPlayers);

			col.Model.AddRange (newPlayers);

			CheckPlayersInLimitedCollectionBy (new int [] { 3, 0, 2 }, players, col);
		}

		[Test]
		public void TestModifyMaxLimitationEnabledWithoutSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = true, Maximum = 3 };
			col.SortByCreationDateDesc = false;
			col.Limitation.Model = ll;
			col.Model.AddRange (players);
			List<Utils.PlayerDummy> newPlayers = new List<Utils.PlayerDummy> ();
			newPlayers.Add (new Utils.PlayerDummy { CreationDate = DateTime.Now.AddMinutes (1), Name = "P4" });
			players.AddRange (newPlayers);
			col.Model.AddRange (newPlayers);
			CheckPlayersInLimitedCollectionBy (new int [] { 1, 2, 0 }, players, col);

			ll.Maximum = 2;

			CheckPlayersInLimitedCollectionBy (new int [] { 1, 2 }, players, col);
		}

		[Test]
		public void TestModifyMaxLimitationEnabledWithSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = true, Maximum = 3 };

			col.Limitation.Model = ll;
			col.Model.AddRange (players);
			List<Utils.PlayerDummy> newPlayers = new List<Utils.PlayerDummy> ();
			newPlayers.Add (new Utils.PlayerDummy { CreationDate = DateTime.Now.AddMinutes (1), Name = "P4" });
			players.AddRange (newPlayers);
			col.Model.AddRange (newPlayers);
			CheckPlayersInLimitedCollectionBy (new int [] { 3, 0, 2 }, players, col);

			ll.Maximum = 2;

			CheckPlayersInLimitedCollectionBy (new int [] { 3, 0 }, players, col);
		}

		[Test]
		public void TestModifyEnabledLimitationEnabledWithoutSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = false, Maximum = 3 };
			col.SortByCreationDateDesc = false;
			col.Limitation.Model = ll;
			col.Model.AddRange (players);
			List<Utils.PlayerDummy> newPlayers = new List<Utils.PlayerDummy> ();
			newPlayers.Add (new Utils.PlayerDummy { CreationDate = DateTime.Now.AddMinutes (1), Name = "P4" });
			players.AddRange (newPlayers);
			col.Model.AddRange (newPlayers);
			CheckPlayersInLimitedCollectionBy (new int [] { 1, 2, 0, 3 }, players, col);

			ll.Enabled = true;

			CheckPlayersInLimitedCollectionBy (new int [] { 1, 2, 0 }, players, col);
		}

		[Test]
		public void TestModifyEnabledLimitationEnabledWithSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = false, Maximum = 3 };

			col.Limitation.Model = ll;
			col.Model.AddRange (players);
			List<Utils.PlayerDummy> newPlayers = new List<Utils.PlayerDummy> ();
			newPlayers.Add (new Utils.PlayerDummy { CreationDate = DateTime.Now.AddMinutes (1), Name = "P4" });
			players.AddRange (newPlayers);
			col.Model.AddRange (newPlayers);
			CheckPlayersInLimitedCollectionBy (new int [] { 3, 0, 2, 1 }, players, col);

			ll.Enabled = true;

			CheckPlayersInLimitedCollectionBy (new int [] { 3, 0, 2 }, players, col);
		}

		[Test]
		public void TestModifyEnabledLimitationEnableReversedWithoutSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = true, Maximum = 3 };
			col.SortByCreationDateDesc = false;
			col.Limitation.Model = ll;
			col.Model.AddRange (players);
			List<Utils.PlayerDummy> newPlayers = new List<Utils.PlayerDummy> ();
			newPlayers.Add (new Utils.PlayerDummy { CreationDate = DateTime.Now.AddMinutes (1), Name = "P4" });
			players.AddRange (newPlayers);
			col.Model.AddRange (newPlayers);
			CheckPlayersInLimitedCollectionBy (new int [] { 1, 2, 0 }, players, col);

			ll.Enabled = false;

			CheckPlayersInLimitedCollectionBy (new int [] { 1, 2, 0, 3 }, players, col);

		}

		[Test]
		public void TestModifyEnabledLimitationEnabledReverseWithSort ()
		{
			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = true, Maximum = 3 };

			col.Limitation.Model = ll;
			col.Model.AddRange (players);
			List<Utils.PlayerDummy> newPlayers = new List<Utils.PlayerDummy> ();
			newPlayers.Add (new Utils.PlayerDummy { CreationDate = DateTime.Now.AddMinutes (1), Name = "P4" });
			players.AddRange (newPlayers);
			col.Model.AddRange (newPlayers);
			CheckPlayersInLimitedCollectionBy (new int [] { 3, 0, 2 }, players, col);

			ll.Enabled = false;

			CheckPlayersInLimitedCollectionBy (new int [] { 3, 0, 2, 1 }, players, col);
		}

		[Test]
		public void TestNotifyCollection ()
		{
			// Arrange
			NotifyCollectionChangedEventArgs receivedEvent = null;
			bool limitedEqualsViewModel = false;
			bool getNotifyEqualsViewModel = false;
			bool secondViewModelEqualsViewModel = false;

			CountLicenseLimitation ll = new CountLicenseLimitation { Enabled = true, Maximum = 3 };
			col.Limitation.Model = ll;

			col.ViewModels.CollectionChanged += (sender, e) => receivedEvent = e;
			col.LimitedViewModels.CollectionChanged += (sender, e) =>
				limitedEqualsViewModel = e == receivedEvent &&
				sender == col.LimitedViewModels;
			col.GetNotifyCollection ().CollectionChanged += (sender, e) =>
				getNotifyEqualsViewModel = e == receivedEvent &&
				sender == col.LimitedViewModels;
			col.ViewModels.CollectionChanged += (sender, e) =>
				secondViewModelEqualsViewModel = e == receivedEvent &&
				sender == col.LimitedViewModels;

			//Act
			col.Model.AddRange (players);

			// Assert
			Assert.AreEqual (players.Count, col.LimitedViewModels.Count);
			Assert.IsNotNull (receivedEvent);

			Assert.IsTrue (limitedEqualsViewModel);
			Assert.IsTrue (getNotifyEqualsViewModel);
			Assert.IsTrue (secondViewModelEqualsViewModel);
		}

		[Test]
		public void ViewModelsCollectionChange_Add_UpdateModelsAndLimitationOk ()
		{
			// Arrange
			LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> collection = new LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> ();
			collection.Limitation = new LicenseLimitationVM { Model = new CountLicenseLimitation { Maximum = 1, Enabled = true } };

			// Act
			collection.ViewModels.Add (new DummyPlayerVM { Model = new Utils.PlayerDummy () });
			collection.ViewModels.Add (new DummyPlayerVM { Model = new Utils.PlayerDummy () });

			// Assert
			Assert.AreEqual (2, collection.Model.Count);
			Assert.AreEqual (1, collection.ViewModels.Count);
			Assert.AreEqual (2, ((CollectionViewModel<Utils.PlayerDummy, DummyPlayerVM>)collection).ViewModels.Count);
			Assert.AreEqual (1, collection.LimitedViewModels.Count);
		}

		[Test]
		public void ViewModelsCollectionChange_Remove_UpdateModelsAndLimitationOk ()
		{
			// Arrange
			LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> collection = new LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> ();
			collection.Limitation = new LicenseLimitationVM { Model = new CountLicenseLimitation { Maximum = 1, Enabled = true } };
			collection.ViewModels.Add (new DummyPlayerVM { Model = new Utils.PlayerDummy () });
			collection.ViewModels.Add (new DummyPlayerVM { Model = new Utils.PlayerDummy () });

			// Act
			var vmToRemove = collection.ViewModels.First ();
			collection.ViewModels.Remove (vmToRemove);

			// Assert
			Assert.AreEqual (1, collection.Model.Count);
			Assert.AreEqual (1, collection.ViewModels.Count);
			Assert.AreEqual (1, ((CollectionViewModel<Utils.PlayerDummy, DummyPlayerVM>)collection).ViewModels.Count);
			Assert.AreEqual (1, collection.LimitedViewModels.Count);
			Assert.IsFalse (collection.ViewModels.Contains (vmToRemove));
		}

		[Test]
		public void RemoveModelFromCollection_VMAddedToLimitedVM_UpdatedOk ()
		{
			// Arrange
			LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> collection = new LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> ();
			collection.Limitation = new LicenseLimitationVM { Model = new CountLicenseLimitation { Maximum = 1, Enabled = true } };
			collection.ViewModels.Add (new DummyPlayerVM { Model = new Utils.PlayerDummy () });
			collection.ViewModels.Add (new DummyPlayerVM { Model = new Utils.PlayerDummy () });

			// Act
			var vmToRemove = collection.ViewModels.First ();
			collection.Model.Remove (vmToRemove.Model);

			// Assert
			Assert.AreEqual (1, collection.Model.Count);
			Assert.AreEqual (1, collection.ViewModels.Count);
			Assert.AreEqual (1, ((CollectionViewModel<Utils.PlayerDummy, DummyPlayerVM>)collection).ViewModels.Count);
			Assert.AreEqual (1, collection.LimitedViewModels.Count);
			Assert.IsFalse (collection.ViewModels.Contains (vmToRemove));
		}

		[Test]
		public void ReplaceModelFromCollection_VMAddedToLimitedVM_UpdatedOk ()
		{
			// Arrange
			LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> collection = new LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> ();
			collection.Limitation = new LicenseLimitationVM { Model = new CountLicenseLimitation { Maximum = 1, Enabled = true } };
			collection.ViewModels.Add (new DummyPlayerVM { Model = new Utils.PlayerDummy () });
			collection.ViewModels.Add (new DummyPlayerVM { Model = new Utils.PlayerDummy () });

			// Act
			var model = new Utils.PlayerDummy ();
			collection.Model.Replace (new List<Utils.PlayerDummy> { model });

			// Assert
			Assert.AreEqual (1, collection.Model.Count);
			Assert.AreEqual (1, collection.ViewModels.Count);
			Assert.AreEqual (1, ((CollectionViewModel<Utils.PlayerDummy, DummyPlayerVM>)collection).ViewModels.Count);
			Assert.AreEqual (1, collection.LimitedViewModels.Count);
			Assert.IsTrue (collection.ViewModels.Any (x => x.Model == model));
		}

		[Test]
		public void ViewModelsCollectionChange_Replace_UpdateModelsAndLimitationOk ()
		{
			// Arrange
			LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> collection = new LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> ();
			collection.Limitation = new LicenseLimitationVM { Model = new CountLicenseLimitation { Maximum = 1, Enabled = true } };
			collection.ViewModels.Add (new DummyPlayerVM { Model = new Utils.PlayerDummy () });
			collection.ViewModels.Add (new DummyPlayerVM { Model = new Utils.PlayerDummy () });

			// Act
			var vmToReplace = new DummyPlayerVM { Model = new Utils.PlayerDummy () };
			collection.ViewModels.Replace (new List<DummyPlayerVM> { vmToReplace });

			// Assert
			Assert.AreEqual (1, collection.Model.Count);
			Assert.AreEqual (1, collection.ViewModels.Count);
			Assert.AreEqual (1, ((CollectionViewModel<Utils.PlayerDummy, DummyPlayerVM>)collection).ViewModels.Count);
			Assert.AreEqual (1, collection.LimitedViewModels.Count);
			Assert.IsTrue (collection.ViewModels.Contains (vmToReplace));
		}

		List<Utils.PlayerDummy> CreateDummyPlayers ()
		{
			List<Utils.PlayerDummy> players = new List<Utils.PlayerDummy> ();
			players.Add (new Utils.PlayerDummy { CreationDate = DateTime.Now, Name = "P1" });
			players.Add (new Utils.PlayerDummy { CreationDate = DateTime.Now.AddMinutes (-2), Name = "P2" });
			players.Add (new Utils.PlayerDummy { CreationDate = DateTime.Now.AddMinutes (-1), Name = "P3" });
			return players;
		}

		public void CheckPlayersInLimitedCollectionBy (
			int [] orderPlayers, List<Utils.PlayerDummy> players,
			LimitedCollectionViewModel<Utils.PlayerDummy, DummyPlayerVM> collection)
		{
			Assert.AreEqual (orderPlayers.Length, collection.Count ());
			for (int i = 0; i < orderPlayers.Length; i++) {
				Assert.AreEqual (players [orderPlayers [i]].Name, collection.ElementAt (i).Model.Name);
			}
		}
	}
}
