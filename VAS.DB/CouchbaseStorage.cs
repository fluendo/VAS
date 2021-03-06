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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Couchbase.Lite;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using VAS.Core;
using VAS.Core.Common;
using VAS.Core.Events;
using VAS.Core.Filters;
using VAS.Core.Interfaces;
using VAS.Core.MVVMC;
using VAS.Core.Serialization;
using VAS.Core.Store;
using VAS.Core.Store.Playlists;
using VAS.Core.Store.Templates;
using VAS.DB.Views;

namespace VAS.DB
{
	public abstract class CouchbaseStorage : DisposableBase, IStorage
	{
		Database db;
		Dictionary<Type, object> views;
		string storageName;
		object mutex;
		bool documentUpdated;
		string dbDir;
		CouchbaseManager ownedManager;

		public CouchbaseStorage (Database db)
		{
			this.db = db;
			Init ();
		}

		public CouchbaseStorage (CouchbaseManager manager, string storageName)
		{
			this.storageName = storageName;
			db = manager.OpenDatabase (storageName);
			Init ();
		}

		public CouchbaseStorage (string dbDir, string storageName)
		{
			this.storageName = storageName;
			this.dbDir = dbDir;
			Start ();
		}

		protected override void DisposeManagedResources ()
		{
			if (ownedManager != null) {
				ownedManager.Dispose ();
			}
			db.Dispose ();
		}

		void Init ()
		{
			views = new Dictionary<Type, object> ();
			// Only keep one revision for each document until we support replication and can handle conflicts
			db.SetMaxRevTreeDepth (1);
			mutex = new object ();
			FetchInfo ();
			BackupAndCompactIfNeeded ();
			InitializeViews ();
			InitializeDocumentTypeMappings ();
		}

		#region IStorage implementation

		internal Database Database {
			get {
				return db;
			}
		}

		/// <summary>
		/// Gets the storage information.
		/// </summary>
		public StorageInfo Info {
			get;
			set;
		}

		abstract protected Version Version {
			get;
		}

		DateTime LastBackup {
			get {
				return Info.LastBackup;
			}
			set {
				Info.LastBackup = value;
			}
		}

		/// <summary>
		/// Retrieves a new instance of the current storage and initializes it again.
		/// </summary>
		public void Start ()
		{
			ownedManager = (CouchbaseManager)App.Current.DependencyRegistry.Retrieve<IStorageManager> (InstanceType.New, dbDir);
			db = ownedManager.OpenDatabase (storageName);
			Init ();
		}

		/// <summary>
		/// Backup this storage to a sigle compressed file.
		/// </summary>
		public bool Backup ()
		{
			try {
				string outputFilename = Path.Combine (App.Current.DBDir, storageName + ".tar.gz");
				using (FileStream fs = new FileStream (outputFilename, FileMode.Create, FileAccess.Write, FileShare.None)) {
					using (Stream gzipStream = new GZipOutputStream (fs)) {
						using (TarArchive tarArchive = TarArchive.CreateOutputTarArchive (gzipStream)) {
							AddDirectoryFilesToTar (tarArchive, Path.Combine (db.Manager.Directory, storageName + ".cblite2"), true);
						}
					}
				}
				LastBackup = DateTime.UtcNow;
				Store (Info);
			} catch (Exception ex) {
				Log.Exception (ex);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Check whether the object of type T exists in the storage.
		/// </summary>
		/// <param name="t">Object to check.</param>
		/// <typeparam name="T">Type of the object to check.</typeparam>
		public bool Exists<T> (T t) where T : IStorable
		{
			// FIXME: add faster API to storage for that or index ID's
			return Retrieve<T> (new QueryFilter ()).Any (p => p.ID == t.ID);
		}

		public object Retrieve (Type type, Guid id)
		{
			return DocumentsSerializer.LoadObject (type, id, db);
		}

		/// <summary>
		/// Fills a partial storable object returned from a query.
		/// </summary>
		/// <param name="storable">the object to fill.</param>
		public void Fill (IStorable storable)
		{
			lock (mutex) {
				bool success = db.RunInTransaction (() => {
					try {
						DocumentsSerializer.FillObject (storable, db);
						return true;
					} catch (Exception ex) {
						Log.Exception (ex);
						return false;
					}
				});
				if (!success) {
					throw new StorageException (Catalog.GetString ("Error filling object from the storage"));
				}
			}
		}

		/// <summary>
		/// Retrieve every object of type T, where T must implement IStorable
		/// </summary>
		/// <typeparam name="T">The type of IStorable you want to retrieve.</typeparam>
		public IEnumerable<T> RetrieveAll<T> () where T : IStorable
		{
			lock (mutex) {
				IQueryView<T> qview = views [typeof (T)] as IQueryView<T>;
				return qview.Query (null);
			}
		}

		/// <summary>
		/// Retrieve an object with the specified id.
		/// </summary>
		/// <param name="id">The object unique identifier.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Retrieve<T> (Guid id) where T : IStorable
		{
			lock (mutex) {
				return (T)Retrieve (typeof (T), id);
			}
		}

		/// <summary>
		/// Retrieve every object of type T, where T must implement IStorable using on the dictionary as a filter on its properties
		/// </summary>
		/// <typeparam name="T">The type of IStorable you want to retrieve.</typeparam>
		/// <param name="filter">The filter used to retrieve the objects</param>
		public IEnumerable<T> Retrieve<T> (QueryFilter filter = null) where T : IStorable
		{
			lock (mutex) {
				IQueryView<T> qview = views [typeof (T)] as IQueryView<T>;
				return qview.Query (filter);
			}
		}

		/// <summary>
		/// Retrieve every object of type T, where T must implement IStorable using on the dictionary as a filter on its properties
		/// </summary>
		/// <typeparam name="T">The type of IStorable you want to retrieve.</typeparam>
		/// <param name="filter">The filter used to retrieve the objects</param>
		/// <param name="cache">An objects cache to reuse existing retrieved objects</param>
		public IEnumerable<T> RetrieveFull<T> (QueryFilter filter = null, IStorableObjectsCache cache = null) where T : IStorable
		{
			lock (mutex) {
				IQueryView<T> qview = views [typeof (T)] as IQueryView<T>;
				return qview.QueryFull (filter, cache);
			}
		}

		/// <summary>
		/// Count all the instances of T in the storage.
		/// </summary>
		/// <typeparam name="T">The type to count in the storage.</typeparam>
		public int Count<T> () where T : IStorable
		{
			return Count<T> (null);
		}

		/// <summary>
		/// Count the instances of T in the storage using the specified filter.
		/// </summary>
		/// <param name="filter">Filter.</param>
		/// <typeparam name="T">The type to count in the storage.</typeparam>
		public int Count<T> (QueryFilter filter) where T : IStorable
		{
			lock (mutex) {
				IQueryView<T> qview = views [typeof (T)] as IQueryView<T>;
				return qview.Count (filter);
			}
		}

		/// <summary>
		/// Store the specified object
		/// </summary>
		/// <param name="t">The object to store.</param>
		/// <param name="forceUpdate">Update all children  ignoring the <see cref="!:IStorable.IsChanged" /> flag.</param>
		/// <typeparam name="T">The type of the object to store.</typeparam>
		public void Store<T> (T t, bool forceUpdate = false) where T : IStorable
		{
			Store<T> (t.ToEnumerable (), forceUpdate);
		}

		/// <summary>
		/// Store a collection of specified objects
		/// </summary>
		/// <param name="storableEnumerable">The objects collection to store.</param>
		/// <param name="forceUpdate">Update all children  ignoring the <see cref="!:IStorable.IsChanged" /> flag.</param>
		/// <typeparam name="T">The type of the objects to store.</typeparam>
		public void Store<T> (IEnumerable<T> storableEnumerable, bool forceUpdate = false) where T : IStorable
		{
			List<T> newDBObjects = new List<T> ();
			lock (mutex) {
				bool success = db.RunInTransaction (() => {
					foreach (var t in storableEnumerable) {
						documentUpdated = false;
						try {

							var doc = db.GetExistingDocument (t.ID.ToString ());
							if (doc == null) {
								forceUpdate = true;
								newDBObjects.Add (t);
							}
							StorableNode node;
							ObjectChangedParser parser = new ObjectChangedParser ();
							parser.Parse (out node, t, Serializer.JsonSettings);

							if (!forceUpdate) {
								Update (node);
							}
							if (forceUpdate) {
								DocumentsSerializer.SaveObject (t, db, saveChildren: true);
							}
							if (t.ID != Info.ID && (forceUpdate || documentUpdated)) {
								Info.LastModified = DateTime.UtcNow;
								DocumentsSerializer.SaveObject (Info, db);
							}
							foreach (IStorable storable in node.OrphanChildren) {
								db.GetDocument (DocumentsSerializer.StringFromID (storable.ID, t.ID)).Delete ();
							}
						} catch (Exception ex) {
							Log.Exception (ex);
							return false;
						}
					}
					return true;
				});
				if (!success) {
					throw new StorageException (Catalog.GetString ("Error storing object from the storage"));
				}
			}
			foreach (var newObject in newDBObjects) {
				// FIXME: StorageDeletedEvent should use a collection instead of having to send one event per storable
				App.Current.EventsBroker.Publish (new StorageAddedEvent<T> { Object = newObject, Sender = this });
			}
		}

		/// <summary>
		/// Delete the specified storable object from the database.
		/// If the object is configured to delete its children,
		/// we perform a bulk deletion of all the documents with the the storable.ID prefix, which is faster than
		/// parsing the object and it ensures that we don't leave orphaned documents in the DB
		/// </summary>
		/// <param name="storable">The object to delete.</param>
		/// <typeparam name="T">The type of the object to delete.</typeparam>
		public void Delete<T> (T storable) where T : IStorable
		{
			Delete (storable.ToEnumerable ());
		}

		/// <summary>
		/// Delete the specified collection of storable object from the database.
		/// If the object is configured to delete its children, we perform a bulk deletion of all the documents with
		/// the the storable.ID prefix, which is faster than parsing the object and it ensures that we don't leave
		/// orphaned documents in the DB
		/// </summary>
		/// <param name="storables">The objects to delete.</param>
		/// <typeparam name="T">The type of the object to delete.</typeparam>
		public void Delete<T> (IEnumerable<T> storables) where T : IStorable
		{
			lock (mutex) {
				bool success = db.RunInTransaction (() => {
					try {
						foreach (T storable in storables) {
							if (storable.DeleteChildren) {
								Query query = db.CreateAllDocumentsQuery ();
								// This should work, but raises an Exception in Couchbase.Lite
								//query.StartKey = t.ID;
								//query.EndKey = t.ID + "\uefff";

								// In UTF-8, from all the possible values in a GUID string, '0' is the first char in the
								// list and 'f' would be the last one
								string sepchar = DocumentsSerializer.ID_SEP_CHAR.ToString ();
								query.StartKey = storable.ID + sepchar + "00000000-0000-0000-0000-000000000000";
								query.EndKey = storable.ID + sepchar + "ffffffff-ffff-ffff-ffff-ffffffffffff";
								query.InclusiveEnd = true;
								foreach (var row in query.Run ()) {
									row.Document.Delete ();
								}
							}
							db.GetDocument (storable.ID.ToString ()).Delete ();
						}
						return true;
					} catch (Exception ex) {
						Log.Exception (ex);
						return false;
					}
				});
				if (success) {
					Info.LastModified = DateTime.UtcNow;
					DocumentsSerializer.SaveObject (Info, db, null, false);
					// FIXME: StorageDeletedEvent should use a collection instead of having to send one event per storable
					foreach (T storable in storables) {
						App.Current.EventsBroker.Publish (new StorageDeletedEvent<T> { Object = storable, Sender = this });
					}
				} else {
					throw new StorageException (Catalog.GetString ("Error deleting objects from the storage"));
				}
			}
		}

		/// <summary>
		/// Reset this instance. Basically will reset the storage to its initial state.
		/// On a FS it can mean to remove every file. On a DB it can mean to remove every entry.
		/// Make sure you know what you are doing before using this.
		/// </summary>
		public void Reset ()
		{
			lock (mutex) {
				db.Delete ();
				db.Manager.ForgetDatabase (db);
			}
		}

		void Delete (StorableNode node, Guid rootID)
		{
			Guid id = node.Storable.ID;
			db.GetDocument (DocumentsSerializer.StringFromID (id, rootID)).Delete ();
			foreach (StorableNode child in node.Children) {
				Delete (child, rootID);
			}
		}

		void Update (StorableNode node, SerializationContext context = null)
		{
			if (context == null) {
				context = new SerializationContext (db, node.Storable.GetType ());
				context.RootID = node.Storable.ID;
			}
			if (node.IsChanged) {
				documentUpdated = true;
				DocumentsSerializer.SaveObject (node.Storable, db, context, false);
			}

			/* Since we are saving single objects manually, we need to keep the stack
			 * updated to avoid problems with circular references */
			context.Stack.Push (node.Storable);
			foreach (StorableNode child in node.Children) {
				Update (child, context);
			}
			context.Stack.Pop ();
		}

		#endregion

		public void AddView (Type t, object obj)
		{
			if (!views.ContainsKey (t)) {
				views.Add (t, obj);
			}
		}

		void FetchInfo ()
		{
			Info = Retrieve<StorageInfo> (Guid.Empty);
			if (Info == null) {
				Info = new StorageInfo {
					Name = storageName,
					LastBackup = DateTime.UtcNow,
					LastCleanup = DateTime.UtcNow,
					Version = Version,
					LastModified = DateTime.UtcNow
				};
				Store (Info);
			}
		}

		void Compact ()
		{
			db.Compact ();
			Info.LastCleanup = DateTime.UtcNow;
			Store (Info);
		}

		protected virtual void InitializeViews ()
		{
			AddView (typeof (Dashboard), new DashboardsView (this));
			AddView (typeof (Project), new ProjectsView (this));
			AddView (typeof (TimelineEvent), new TimelineEventsView (this));
			AddView (typeof (EventType), new EventTypeView (this));
			AddView (typeof (Playlist), new PlaylistView (this));
		}

		protected virtual void InitializeDocumentTypeMappings ()
		{
			Dictionary<Type, string> typesToDocumentTypes = new Dictionary<Type, string> ();
			foreach (IQueryView view in views.Values) {
				typesToDocumentTypes.Add (view.Type, view.DocumentType);
			}
			DocumentsSerializer.DocumentTypeBaseTypes = typesToDocumentTypes;
		}

		void AddDirectoryFilesToTar (TarArchive tarArchive, string sourceDirectory, bool recurse)
		{
			// Recursively add sub-folders
			if (recurse) {
				string [] directories = Directory.GetDirectories (sourceDirectory);
				foreach (string directory in directories)
					AddDirectoryFilesToTar (tarArchive, directory, recurse);
			}

			// Add files
			string [] filenames = Directory.GetFiles (sourceDirectory);
			foreach (string filename in filenames) {
				TarEntry tarEntry = TarEntry.CreateEntryFromFile (filename);
				tarArchive.WriteEntry (tarEntry, true);
			}
		}

		/// <summary>
		/// Backup and Compact the database if needed.
		/// </summary>
		void BackupAndCompactIfNeeded ()
		{
			if ((Info.LastModified - Info.LastBackup).TotalDays > 2) {
				Backup ();
			}
			if ((Info.LastModified - Info.LastCleanup).TotalDays > 2) {
				Compact ();
			}
		}
	}
}

