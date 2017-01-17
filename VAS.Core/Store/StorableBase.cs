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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using VAS.Core.Common;
using VAS.Core.Interfaces;
using VAS.Core.MVVMC;
using VAS.Core.Serialization;

namespace VAS.Core.Store
{
	[Serializable]
	public class StorableBase : BindableBase, IStorable
	{
		[NonSerialized]
		IStorage storage;

		DateTime creationDate;

		public StorableBase ()
		{
			IsLoaded = true;
			CreationDate = DateTime.UtcNow;
		}

		#region IStorable implementation

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public bool IsLoaded {
			get;
			set;
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		// Use IgnoreDataMember to prevent the cloner trying to serialize Storage
		public IStorage Storage {
			get {
				return storage;
			}
			set {
				storage = value;
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public virtual bool DeleteChildren {
			get {
				return true;
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public List<IStorable> SavedChildren {
			get;
			set;
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public string DocumentID {
			get;
			set;
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public Guid ParentID {
			get;
			set;
		}

		#endregion

		#region IIDObject implementation

		[JsonProperty (Order = -100)]
		public virtual Guid ID {
			get;
			set;
		}

		#endregion

		/// <summary>
		/// Gets or sets the creation date. It's stored in order to have a sorting condition.
		/// </summary>
		/// <value>The creation date.</value>
		[PropertyPreload]
		public DateTime CreationDate {
			get {
				return creationDate;
			}
			set {
				creationDate = value.ToUniversalTime ();
			}
		}

		/// <summary>
		/// Set to <c>true</c> while the object is being loaded. Used internally
		/// to prevent infinite loops.
		/// </summary>
		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		bool IsLoading {
			get;
			set;
		}

		protected virtual void CheckIsLoaded ()
		{
			if (!IsLoaded && !IsLoading && !Disposed) {
				IsLoading = true;
				if (Storage == null) {
					throw new StorageException ("Storage not set in preloaded object");
				}
				Storage.Fill (this);
				IsLoaded = true;
				IsLoading = false;
			}
		}

		public void Load ()
		{
			CheckIsLoaded ();
		}

		public override bool Equals (object obj)
		{
			StorableBase s = obj as StorableBase;
			return s != null && ID.Equals (s.ID);
		}

		public override int GetHashCode ()
		{
			return ID.GetHashCode ();
		}

	}
}

