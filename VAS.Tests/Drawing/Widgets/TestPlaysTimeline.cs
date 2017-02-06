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
using System.Linq;
using Moq;
using NUnit.Framework;
using VAS.Core.Common;
using VAS.Core.Interfaces.Drawing;
using VAS.Core.MVVMC;
using VAS.Core.Store;
using VAS.Core.ViewModel;
using VAS.Drawing.CanvasObjects.Timeline;
using VAS.Drawing.Widgets;

namespace VAS.Tests.Drawing.Widgets
{

	[TestFixture]
	public class TestPlaysTimeline
	{
		Project project;
		PlaysTimeline timeline;
		ProjectVM projectVM;
		Mock<IWidget> widgetMock;

		[TestFixtureSetUp]
		public void Init ()
		{
			App.Current.ViewLocator = new ViewLocator ();
			App.Current.ViewLocator.Register ("TimelineEventView", typeof (DummyTimelineEventView));
			App.Current.DrawingToolkit = new Mock<IDrawingToolkit> ().Object;
		}

		[SetUp]
		public void SetUp ()
		{
			project = Utils.CreateProject (true);
			project.Periods.Add (new Period ());
			projectVM = new ProjectVM { Model = project };
			widgetMock = new Mock<IWidget> ();
			widgetMock.SetupAllProperties ();
			timeline = new PlaysTimeline (widgetMock.Object);
			var viewModel = new DummyAnalysisVM { Project = projectVM };
			timeline.SetViewModel (viewModel);
		}

		[Test]
		public void TestCreateTimeline ()
		{
			Assert.AreEqual (5, timeline.Objects.OfType<EventTypeTimelineView> ().Count ());
		}

		[Test]
		public void TestAddEventType ()
		{
			projectVM.Timeline.EventTypesTimeline.ViewModels.Add (new EventTypeTimelineVM { Model = new EventType { Name = "EV" } });

			Assert.AreEqual (6, timeline.Objects.OfType<EventTypeTimelineView> ().Count ());
		}

		[Test]
		public void TestRemoveEventType ()
		{
			projectVM.Timeline.EventTypesTimeline.ViewModels.Remove (projectVM.Timeline.EventTypesTimeline.ViewModels.First ());

			Assert.AreEqual (4, timeline.Objects.OfType<EventTypeTimelineView> ().Count ());
		}

		[Test]
		public void TestUpdateDuration ()
		{
			project.FileSet [0].Duration = new Time { TotalSeconds = 400 };

			double width = project.FileSet.Duration.TotalSeconds / timeline.SecondsPerPixel + 10;
			Assert.AreEqual (widgetMock.Object.Width, width);
			foreach (EventTypeTimelineView view in timeline.Objects.OfType<EventTypeTimelineView> ()) {
				Assert.AreEqual (width, view.Width);
				Assert.AreEqual (project.FileSet.Duration, view.Duration);
			}
		}

		[Test]
		public void TestClear ()
		{
			projectVM.Timeline.EventTypesTimeline.ViewModels.Clear ();

			Assert.AreEqual (0, timeline.Objects.OfType<EventTypeTimelineView> ().Count ());
		}

		[Test]
		public void TestReplace ()
		{
			projectVM.Timeline.EventTypesTimeline.ViewModels.Replace (
				new EventTypeTimelineVM (new EventTypeVM { Model = new EventType { Name = "TEST" } }).ToEnumerable ());

			Assert.AreEqual (1, timeline.Objects.OfType<EventTypeTimelineView> ().Count ());
		}
	}
}
