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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using VAS.Core.Common;
using VAS.Core.Interfaces;
using VAS.Core.Serialization;

namespace VAS.Core.Store
{
	[Serializable]
	[JsonObject]
	public class MediaFileSet : StorableBase, IList<MediaFile>
	{
		[NonSerialized]
		IStorage storage;
		bool isLoading;

		public MediaFileSet ()
		{
			ID = Guid.NewGuid ();
			VisibleRegion = null;
			MediaFiles = new RangeObservableCollection<MediaFile> ();
		}

		/// <summary>
		/// Gets or sets the visible region of the file set.
		/// </summary>
		/// <value>The visible region.</value>
		public TimeNode VisibleRegion {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets if the media file is stretched.
		/// </summary>
		/// <value>The is stretched.</value>
		public bool IsStretched {
			get;
			set;
		}

		public RangeObservableCollection<MediaFile> MediaFiles {
			get;
			set;
		}

		// Keep this property for backwards compatibility
		[JsonProperty]
		[Obsolete]
		Dictionary<MediaFileAngle, MediaFile> Files {
			set {
				// Transform old Files dict to ordered list
				foreach (KeyValuePair<MediaFileAngle, MediaFile> File in value) {
					if (File.Value != null) {
						// Set the angle as the name
						switch (File.Key) {
						case MediaFileAngle.Angle1:
							File.Value.Name = Catalog.GetString ("Main camera angle");
							break;
						case MediaFileAngle.Angle2:
							File.Value.Name = Catalog.GetString ("Angle 2");
							break;
						case MediaFileAngle.Angle3:
							File.Value.Name = Catalog.GetString ("Angle 3");
							break;
						case MediaFileAngle.Angle4:
							File.Value.Name = Catalog.GetString ("Angle 4");
							break;
						}
						// Add to list
						Add (File.Value);
					}
				}
				// FIXME: Order list
			}
		}

		/// <summary>
		/// Gets the preview of the first file in set or null if the set is empty.
		/// </summary>
		/// <value>The preview.</value>
		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public Image Preview {
			get {
				MediaFile file = this.FirstOrDefault ();

				if (file != null) {
					return file.Preview;
				} else {
					return null;
				}
			}
		}

		/// <summary>
		/// Gets the maximum duration from all files in set.
		/// </summary>
		/// <value>The duration.</value>
		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public Time Duration {
			get {
				if (Count != 0) {
					return this.Max (mf => mf == null ? new Time (0) : mf.Duration);
				} else {
					return new Time (0);
				}
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public int Count {
			get {
				return MediaFiles.Count;
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public bool IsReadOnly {
			get {
				return false;
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public MediaFile this [int index] {
			get {
				return MediaFiles [index];
			}
			set {
				MediaFiles [index] = value;
			}
		}

		/// <summary>
		/// Search for first file with matching name and replace with new file.
		/// If no file with matching name was found, this is equivalent to adding new file to the set.
		/// Note that new file does not have to use the same name.
		/// </summary>
		/// <param name="name">Name to use for the search.</param>
		/// <param name="file">File.</param>
		/// <returns><c>true</c> if the name was found and a substitution happened, <c>false</c> otherwise.</returns>
		public bool Replace (String name, MediaFile file)
		{
			MediaFile old_file = this.Where (mf => mf.Name == name).FirstOrDefault ();
			return Replace (old_file, file);
		}

		/// <summary>
		/// Search for a file in the set and replace it with a new one.
		/// If old file is not found, this is equivalent to adding new file to the set.
		/// Some properties from the old file are copied to the new file such as Name and Offset.
		/// </summary>
		/// <param name="old_file">Old file.</param>
		/// <param name="new_file">New file.</param>
		/// <returns>><c>true</c> if the old file was found and a substitution happened, <c>false</c> otherwise.</returns>
		public bool Replace (MediaFile old_file, MediaFile new_file)
		{
			bool found = false;

			if (Contains (old_file)) {
				if (new_file != null && old_file != null) {
					new_file.Name = old_file.Name;
					new_file.Offset = old_file.Offset;
				}

				this [IndexOf (old_file)] = new_file;
				found = true;
			} else {
				Add (new_file);
			}

			return found;
		}

		/// <summary>
		/// Checks that all files in the set are valid.
		/// </summary>
		/// <param name="searchPath">Path where the project file is (during import). 
		/// If != null, try to find the mediaFile in this path</param>
		/// <returns><c>true</c>, if files was checked, <c>false</c> otherwise.</returns>
		public bool CheckFiles (string searchPath = null)
		{
			if (Count == 0) {
				return false;
			}
			foreach (MediaFile f in this) {
				if (!f.Exists ()) {
					if (searchPath == null) {
						return false;
					}

					// Try to find the mediafiles in the search path
					string file = Path.GetFileName (f.FilePath);
					string newPath = Path.Combine (searchPath, file);
					if (App.Current.FileSystemManager.FileExists (newPath)) {
						f.FilePath = newPath;
					} else {
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Checks that the MediaFileSet passed is same reference
		///  If not checks if the filepaths of boths MediaFileSets
		/// are the same
		/// </summary>
		/// <returns><c>true</c>, if not same reference and mediafiles paths are different, <c>false</c> otherwise.</returns>
		/// <param name="fileset">MediaFileSet to check if its same reference and the filepaths</param>
		public bool CheckMediaFilesModified (MediaFileSet fileset)
		{
			if (!Object.ReferenceEquals (this, fileset)) {
				foreach (MediaFile f in fileset) {
					if (this.Where (fs => fs.FilePath == f.FilePath).Count () <= 0) {
						return true;
					}
				}
			}
			return false;
		}

		public override bool Equals (object obj)
		{
			MediaFileSet s = obj as MediaFileSet;
			return s != null && ID.Equals (s.ID);
		}

		public override int GetHashCode ()
		{
			return ID.GetHashCode ();
		}

		public IEnumerator<MediaFile> GetEnumerator ()
		{
			return MediaFiles.GetEnumerator ();
		}

		public int IndexOf (MediaFile item)
		{
			return MediaFiles.IndexOf (item);
		}

		public void Insert (int index, MediaFile item)
		{
			MediaFiles.Insert (index, item);
		}

		public void RemoveAt (int index)
		{
			MediaFiles.RemoveAt (index);
		}

		public void Add (MediaFile item)
		{
			MediaFiles.Add (item);
		}

		public void Clear ()
		{
			MediaFiles.Clear ();
		}

		public bool Contains (MediaFile item)
		{
			return MediaFiles.Contains (item);
		}

		public void CopyTo (MediaFile [] array, int arrayIndex)
		{
			MediaFiles.CopyTo (array, arrayIndex);
		}

		public bool Remove (MediaFile item)
		{
			return MediaFiles.Remove (item);
		}

		protected override void CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (Count != 0 && Duration != null) {
				if (VisibleRegion == null) {
					VisibleRegion = new TimeNode { Start = new Time (0), Stop = new Time (Duration.MSeconds) };
				} else {
					if (VisibleRegion.Start > Duration) {
						VisibleRegion.Start = new Time (0);
					}
					if (VisibleRegion.Stop > Duration) {
						VisibleRegion.Stop = new Time (Duration.MSeconds);
					}
				}
			}
			base.CollectionChanged (sender, e);
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return MediaFiles.GetEnumerator ();
		}
	}
}

