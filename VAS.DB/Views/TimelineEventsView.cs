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
using System.Collections;
using System.Collections.Generic;
using Couchbase.Lite;
using Newtonsoft.Json.Linq;
using VAS.Core.Store;

namespace VAS.DB.Views
{
	public class TimelineEventsView: GenericView <TimelineEvent>
	{
		public TimelineEventsView (CouchbaseStorage storage) : base (storage)
		{
		}

		protected override object GenKeys (IDictionary<string, object> document)
		{
			var keys = new List<object> ();
			foreach (string propName in FilterProperties.Keys) {
				if ((bool)FilterProperties [propName]) {
					keys.Add (DocumentsSerializer.IDStringFromString (document [propName] as string));
				} else {
					keys.Add (document [propName]);
				}
			}
			return new PropertyKey (keys);
		}

		protected override MapDelegate GetMap (string docType)
		{
			return (document, emitter) => {
				if (docType.Equals (document [DocumentsSerializer.DOC_TYPE])) {
					PropertyKey keys = GenKeys (document) as PropertyKey;
					emitter (keys, GenValue (document));
				}
			};
		}

		protected override string ViewVersion {
			get {
				return "1";
			}
		}
	}
}

