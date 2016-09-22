﻿//
//  Copyright (C) 2016 FLUENDO S.A.
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
using System.Dynamic;
using Moq;
using NUnit.Framework;
using VAS.Core;
using VAS.Core.Events;
using VAS.Core.Interfaces.GUI;

namespace VAS.Tests.Core.Common
{
	[TestFixture]
	public class TestStateController
	{
		StateController sc;
		string lastTransition = null;

		[SetUp]
		public void InitializeStateController ()
		{
			sc = new StateController ();
			App.Current.EventsBroker.Subscribe<NavigationEvent> (HandleTransitionEvent);
			lastTransition = null;
		}

		[TearDown]
		public void Deinitializer ()
		{
			App.Current.EventsBroker.Unsubscribe<NavigationEvent> (HandleTransitionEvent);
		}

		void HandleTransitionEvent (NavigationEvent evt)
		{
			lastTransition = evt.Name;
		}

		IScreenState GetScreenStateDummy (string transitionName)
		{
			var screenStateMock = new Mock<IScreenState> ();
			screenStateMock.Setup (x => x.PreTransition (It.IsAny<ExpandoObject> ())).Returns (AsyncHelpers.Return (true));
			screenStateMock.Setup (x => x.PostTransition ()).Returns (AsyncHelpers.Return (true));
			screenStateMock.Setup (x => x.Panel).Returns (new Mock<IPanel> ().Object);
			screenStateMock.Setup (x => x.Name).Returns (transitionName);
			return screenStateMock.Object;
		}

		[Test]
		public void TestRegister ()
		{
			Assert.DoesNotThrow (() => sc.Register ("newTransition", () => GetScreenStateDummy ("newTransition")));
		}

		[Test]
		public void TestUnRegister ()
		{
			// Arrange
			sc.Register ("newTransition", () => GetScreenStateDummy ("newTransition"));

			// Action
			bool obtained = sc.UnRegister ("newTransition");

			// Assert
			Assert.IsTrue (obtained);
		}

		[Test]
		public async void TestUnRegister_CheckDispose ()
		{
			// Arrange
			string transitionName = "newTransition";
			sc.Register (transitionName, () => GetScreenStateDummy ("newTransition"));
			await sc.MoveTo (transitionName, null);

			// Action
			sc.UnRegister (transitionName);

			// Assert
			bool moveTransition = await sc.MoveBack ();
			Assert.IsFalse (moveTransition);
		}

		[Test]
		public void TestUnRegister_WithOverwrittenTransitions ()
		{
			// Arrange
			sc.Register ("newTransition", () => GetScreenStateDummy ("newTransition"));
			sc.Register ("newTransition", () => GetScreenStateDummy ("newTransition"));

			// Action & assert
			Assert.DoesNotThrow (() => sc.UnRegister ("newTransition"));
		}

		[Test]
		public async void TestMoveTo ()
		{
			// Arrange
			sc.Register ("newTransition", () => GetScreenStateDummy ("newTransition"));

			// Action
			bool moveTransition = await sc.MoveTo ("newTransition", null);

			// Assert
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("newTransition", lastTransition);
		}

		[Test]
		public async void TestMoveToModal ()
		{
			// Arrange
			sc.Register ("home", () => GetScreenStateDummy ("home"));
			sc.Register ("newModalTransition", () => GetScreenStateDummy ("newTransition"));
			await sc.SetHomeTransition ("home", null);
			// Action
			bool moveTransition = await sc.MoveToModal ("newModalTransition", null);

			// Assert
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("newModalTransition", lastTransition);
		}

		[Test]
		public async void TestMoveBack ()
		{
			bool moveTransition;
			// Arrange
			sc.Register ("newTransition", () => GetScreenStateDummy ("newTransition"));
			sc.Register ("newModalTransition", () => GetScreenStateDummy ("newModalTransition"));
			moveTransition = await sc.MoveTo ("newTransition", null);
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("newTransition", lastTransition);
			moveTransition = await sc.MoveToModal ("newModalTransition", null);
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("newModalTransition", lastTransition);
			moveTransition = await sc.MoveBack ();
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("newTransition", lastTransition);
			moveTransition = await sc.MoveBack ();
			Assert.IsFalse (moveTransition);
		}

		[Test]
		public async void TestMoveBackTo ()
		{
			bool moveTransition;
			// Arrange
			sc.Register ("Transition1", () => GetScreenStateDummy ("Transition1"));
			sc.Register ("Transition2", () => GetScreenStateDummy ("Transition2"));
			sc.Register ("newModalTransition", () => GetScreenStateDummy ("newModalTransition"));
			moveTransition = await sc.MoveTo ("Transition1", null);
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("Transition1", lastTransition);
			moveTransition = await sc.MoveTo ("Transition2", null);
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("Transition2", lastTransition);
			moveTransition = await sc.MoveToModal ("newModalTransition", null);
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("newModalTransition", lastTransition);
			moveTransition = await sc.MoveBackTo ("Transition1");
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("Transition1", lastTransition);
		}

		[Test]
		public async void TestMoveToHome ()
		{
			bool moveTransition;
			// Arrange
			sc.Register ("Home", () => GetScreenStateDummy ("Home"));
			sc.Register ("Transition1", () => GetScreenStateDummy ("Transition1"));
			sc.Register ("Transition2", () => GetScreenStateDummy ("Transition2"));
			sc.Register ("newModalTransition", () => GetScreenStateDummy ("newModalTransition"));
			await sc.SetHomeTransition ("Home", null);
			moveTransition = await sc.MoveTo ("Transition1", null);
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("Transition1", lastTransition);
			moveTransition = await sc.MoveTo ("Transition2", null);
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("Transition2", lastTransition);
			moveTransition = await sc.MoveToModal ("newModalTransition", null);
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("newModalTransition", lastTransition);
			moveTransition = await sc.MoveToHome ();
			Assert.IsTrue (moveTransition);
			Assert.AreEqual ("Home", lastTransition);
		}

		[Test]
		public async void TestMoveToAndEmptyStack ()
		{
			// Arrange
			sc.Register ("Home", () => GetScreenStateDummy ("Home"));
			sc.Register ("Transition1", () => GetScreenStateDummy ("Transition1"));
			sc.Register ("Transition2", () => GetScreenStateDummy ("Transition2"));
			sc.Register ("Transition3", () => GetScreenStateDummy ("Transition2"));
			await sc.SetHomeTransition ("Home", null);
			await sc.MoveTo ("Transition1", null);
			await sc.MoveTo ("Transition2", null);

			// Action
			await sc.MoveTo ("Transition3", null, true);
			await sc.MoveBack ();

			// Assert
			Assert.AreEqual (sc.Current, "Home");
		}
	}
}
