﻿//
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
using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using VAS.Core.Common;
using VAS.Core.Serialization;
using VAS.Core.Store;

namespace VAS.Tests.Core.Store
{
	[TestFixture ()]
	public class TestMediaFileSet
	{
		[Test ()]
		public void TestSerialization ()
		{
			MediaFileSet mf = new MediaFileSet ();
			mf.Add (new MediaFile ("path", 34000, 25, true, true, "mp4", "h264",
				"aac", 320, 240, 1.3, null, "Test asset"));
			mf.Add (new MediaFile ("path", 34000, 25, true, true, "mp4", "h264",
				"aac", 320, 240, 1.3, null, "Test asset 2"));
			Utils.CheckSerialization (mf);
		}

		[Test ()]
		public void TestMigration ()
		{
			String old_json = @"{ 
							      ""$id"": ""88"",
							      ""$type"": ""VAS.Core.Store.MediaFileSet, VAS.Core"",
							      ""Files"": { 
							        ""$id"": ""1"",
							        ""$type"": ""System.Collections.Generic.Dictionary`2[[VAS.Core.Common.MediaFileAngle, VAS.Core],[VAS.Core.Store.MediaFile, VAS.Core]], mscorlib"",
							        ""Angle1"": { ""$id"": ""2"", ""$type"": ""VAS.Core.Store.MediaFile, VAS.Core"", ""FilePath"": ""test.mp4"", ""Duration"": null, ""HasVideo"": false, ""HasAudio"": false, ""Container"": null, ""VideoCodec"": null, ""AudioCodec"": null, ""VideoWidth"": 640, ""VideoHeight"": 480, ""Fps"": 25, ""Par"": 1.0, ""Preview"": null, ""Offset"": 0 },
							        ""Angle2"": { ""$id"": ""3"", ""$type"": ""VAS.Core.Store.MediaFile, VAS.Core"", ""FilePath"": ""test2.mp4"", ""Duration"": null, ""HasVideo"": false, ""HasAudio"": false, ""Container"": null, ""VideoCodec"": null, ""AudioCodec"": null, ""VideoWidth"": 640, ""VideoHeight"": 480, ""Fps"": 25, ""Par"": 1.0, ""Preview"": null, ""Offset"": 0 },
							        ""Angle3"": null,
							        ""Angle4"": null
							      }
								}";
			MemoryStream stream = new MemoryStream ();
			StreamWriter writer = new StreamWriter (stream);
			writer.Write (old_json);
			writer.Flush ();
			stream.Position = 0;

			// Deserialize and check the FileSet
			var newobj = Serializer.Instance.Load<MediaFileSet> (stream, SerializationType.Json);

			Assert.AreEqual (2, newobj.Count);

			MediaFile mf = newobj.First ();

			Assert.AreEqual ("test.mp4", mf.FilePath);
			Assert.AreEqual ("Main camera angle", mf.Name);

			mf = newobj [1];

			Assert.AreEqual ("test2.mp4", mf.FilePath);
			Assert.AreEqual ("Angle 2", mf.Name);
		}

		[Test ()]
		public void TestPreview ()
		{
			MediaFileSet mf = new MediaFileSet ();
			Assert.IsNull (mf.Preview);
			mf.Add (new MediaFile { Preview = Utils.LoadImageFromFile (), Name = "Test asset" });
			Assert.IsNotNull (mf.Preview);
		}

		[Test ()]
		public void TestDuration ()
		{
			MediaFileSet mf = new MediaFileSet ();
			Assert.AreEqual (mf.Duration.MSeconds, 0);
			mf.Add (new MediaFile { Duration = new Time (2000), Name = "Test asset" });
			Assert.AreEqual (mf.Duration.MSeconds, 2000);
			mf.Replace ("Test asset", new MediaFile { Duration = new Time (2001), Name = "Test asset 2" });
			Assert.AreEqual (mf.Duration.MSeconds, 2001);
		}

		[Test ()]
		public void TestOrderReplace ()
		{
			MediaFileSet mf = new MediaFileSet ();

			mf.Add (new MediaFile ("path1", 34000, 25, true, true, "mp4", "h264",
				"aac", 320, 240, 1.3, null, "Test asset") { Offset = new Time (1) });
			mf.Add (new MediaFile ("path2", 34000, 25, true, true, "mp4", "h264",
				"aac", 320, 240, 1.3, null, "Test asset 2") { Offset = new Time (2) });
			mf.Add (new MediaFile ("path3", 34000, 25, true, true, "mp4", "h264",
				"aac", 320, 240, 1.3, null, "Test asset 3") { Offset = new Time (3) });

			Assert.AreEqual (3, mf.Count);
			Assert.AreEqual ("path1", mf [0].FilePath);
			Assert.AreEqual ("path2", mf [1].FilePath);
			Assert.AreEqual ("path3", mf [2].FilePath);

			mf.Replace ("Test asset 2", new MediaFile ("path4", 34000, 25, true, true, "mp4", "h264",
				"aac", 320, 240, 1.3, null, "Test asset 4") { Offset = new Time (4) });

			Assert.AreEqual (3, mf.Count);
			Assert.AreEqual ("path1", mf [0].FilePath);
			Assert.AreEqual ("path4", mf [1].FilePath);
			Assert.AreEqual ("Test asset 2", mf [1].Name);
			Assert.AreEqual (new Time (2), mf [1].Offset);
			Assert.AreEqual ("path3", mf [2].FilePath);

			mf.Replace (mf [1], new MediaFile ("path5", 34000, 25, true, true, "mp4", "h264",
				"aac", 320, 240, 1.3, null, "Test asset 5") { Offset = new Time (5) });

			Assert.AreEqual (3, mf.Count);
			Assert.AreEqual ("path1", mf [0].FilePath);
			Assert.AreEqual ("path5", mf [1].FilePath);
			Assert.AreEqual ("Test asset 2", mf [1].Name);
			Assert.AreEqual (new Time (2), mf [1].Offset);
			Assert.AreEqual ("path3", mf [2].FilePath);
		}

		[Test ()]
		public void TestCheckFiles ()
		{
			string path = Path.GetTempFileName ();
			MediaFileSet mf = new MediaFileSet ();
			Assert.IsFalse (mf.CheckFiles ());
			mf.Add (new MediaFile { FilePath = path, Name = "Test asset" });
			try {
				Assert.IsTrue (mf.CheckFiles ());
			} finally {
				File.Delete (path);
			}
		}

		[Test ()]
		public void TestCheckFilesNonExisting ()
		{
			// Arrange file with wrong path
			string path = Path.GetTempFileName ();
			MediaFileSet mf = new MediaFileSet ();
			mf.Add (new MediaFile {
				FilePath = Path.Combine ("non-existing-path", Path.GetFileName (path)),
				Name = "Test asset"
			});
			try {
				// Act & Assert
				Assert.IsFalse (mf.CheckFiles ());
			} finally {
				File.Delete (path);
			}
		}

		[Test ()]
		public void TestCheckFilesFixPath ()
		{
			// Arrange file with wrong path
			string path = Path.GetTempFileName ();
			string originalPath = Path.Combine ("non-existing-path", Path.GetFileName (path));
			MediaFileSet mf = new MediaFileSet ();
			mf.Add (new MediaFile {
				FilePath = originalPath,
				Name = "Test asset"
			});
			try {
				// Act
				bool ret = mf.CheckFiles (Path.GetDirectoryName (path));

				// Assert its path is fixed
				Assert.IsTrue (ret);
				Assert.AreEqual (path, mf.First ().FilePath);
				Assert.AreNotEqual (originalPath, mf.First ().FilePath);
			} finally {
				File.Delete (path);
			}
		}

		[Test ()]
		public void TestCheckFilesPathNotFixed ()
		{
			// Arrange file with wrong path
			string path = Path.GetTempFileName ();
			string wrongDir = "other-non-existing-path";
			string wrongFullPath = Path.Combine (wrongDir, Path.GetFileName (path));
			string originalFullPath = Path.Combine ("non-existing-path", Path.GetFileName (path));

			MediaFileSet mf = new MediaFileSet ();
			mf.Add (new MediaFile {
				FilePath = originalFullPath,
				Name = "Test asset"
			});
			try {
				// Act
				bool ret = mf.CheckFiles (wrongDir);

				// Assert path is not fixed
				Assert.IsFalse (ret);
				Assert.AreNotEqual (wrongFullPath, mf.First ().FilePath);
				Assert.AreEqual (originalFullPath, mf.First ().FilePath);
			} finally {
				File.Delete (path);
			}
		}

		[Test ()]
		public void TestNotEquals ()
		{
			MediaFileSet mf = new MediaFileSet ();
			MediaFileSet mf2 = new MediaFileSet ();

			Assert.IsFalse (mf.Equals (mf2));
		}

		[Test ()]
		public void TestEquals ()
		{
			MediaFileSet mf = new MediaFileSet ();
			MediaFileSet mf2 = new MediaFileSet ();
			mf2.ID = mf.ID;

			Assert.IsTrue (mf.Equals (mf2));
		}

		[Test ()]
		public void TestMediaFileSetPathModifiedSameReferenceReturnsFalse ()
		{
			MediaFileSet mfs = new MediaFileSet ();
			mfs.Add (new MediaFile {
				FilePath = "/videos/test.mp4"
			});
			MediaFileSet mfs2 = mfs;
			mfs2 [0].FilePath = "/videos/test2.mp4";

			Assert.IsFalse (mfs2.CheckMediaFilesModified (mfs));
			Assert.AreEqual (mfs [0].FilePath, mfs2 [0].FilePath);
		}

		[Test ()]
		public void TestMediaFileSetPathModifiedDifferentReferenceReturnsTrue ()
		{
			MediaFileSet mfs = new MediaFileSet ();
			mfs.Add (new MediaFile {
				FilePath = "/videos/test.mp4"
			});

			MediaFileSet mfs2 = mfs.Clone ();
			mfs2 [0].FilePath = "/videos/test2.mp4";

			Assert.IsTrue (mfs2.CheckMediaFilesModified (mfs));
			Assert.AreNotEqual (mfs [0].FilePath, mfs2 [0].FilePath);
			Assert.AreEqual (mfs.ID, mfs2.ID);
		}

		[Test]
		public void TestVisibleRegionRemainsNullAfterClear ()
		{
			MediaFileSet mfs = new MediaFileSet ();
			Assert.IsNull (mfs.VisibleRegion);

			mfs.Clear ();

			Assert.IsNull (mfs.VisibleRegion);
		}

		[Test]
		public void TestVisibleRegionSetAfterAddingFirstElement ()
		{
			MediaFileSet mfs = new MediaFileSet ();
			Assert.IsNull (mfs.VisibleRegion);

			mfs.Add (new MediaFile {
				FilePath = "/videos/test.mp4",
				Duration = new Time (5000),
			});

			Assert.IsNotNull (mfs.VisibleRegion);
			Assert.AreEqual (new Time (0), mfs.VisibleRegion.Start);
			Assert.AreEqual (new Time (5000), mfs.VisibleRegion.Stop);
		}

		[Test]
		public void TestVisibleRegionOutOfBounds ()
		{
			MediaFileSet mfs = new MediaFileSet ();
			mfs.Add (new MediaFile {
				FilePath = "/videos/test.mp4",
				Duration = new Time (20000),
			});
			mfs.VisibleRegion.Start = new Time (7000);
			mfs.VisibleRegion.Stop = new Time (17000);
			mfs.Clear ();
			mfs.Add (new MediaFile {
				FilePath = "/videos/test.mp4",
				Duration = new Time (5000),
			});

			Assert.IsNotNull (mfs.VisibleRegion);
			Assert.AreEqual (new Time (0), mfs.VisibleRegion.Start);
			Assert.AreEqual (new Time (5000), mfs.VisibleRegion.Stop);
		}
	}
}

