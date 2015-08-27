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
using System.Linq;
using Couchbase.Lite;
using LongoMatch.Core.Common;
using LongoMatch.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace LongoMatch.DB
{
	public static class DocumentsSerializer
	{
		public const string DOC_TYPE = "DocType";
		public const string OBJ_TYPE = "ObjType";

		public static void SaveObject (IStorable obj, Database db, SerializationContext context = null)
		{
			if (context == null) {
				context = new SerializationContext (db, obj.GetType ());
			}
			Document doc = db.GetDocument (obj.ID.ToString ());
			doc.Update ((UnsavedRevision rev) => {
				JObject jo = SerializeObject (obj, rev, context);
				IDictionary<string, object> props = jo.ToObject<IDictionary<string, object>> ();

				/* SetProperties sets a new properties dictionary, removing the attachments we
					 * added in the serialization */
				if (rev.Properties.ContainsKey ("_attachments")) {
					props ["_attachments"] = rev.Properties ["_attachments"];
				}
				rev.SetProperties (props);
				return true;
			});
		}

		public static IStorable LoadObject (Type objType, Guid id, Database db, SerializationContext context = null)
		{
			IStorable storable, parent;

			if (context == null) {
				context = new SerializationContext (db, objType);
			}
			Document doc = db.GetExistingDocument (id.ToString ());
			Type realType = Type.GetType (doc.Properties [OBJ_TYPE] as string);
			if (realType == null) {
				/* Should never happen */
				Log.Error ("Error getting type " + doc.Properties [OBJ_TYPE] as string);
				realType = objType;
			}
			storable = DeserializeObject (doc, realType, context) as IStorable;
			if (context.Stack.Count != 0) {
				parent = context.Stack.Peek ();
				parent.SavedChildren.Add (storable);
			}
			return storable;
		}

		public static void FillObject (IStorable storable, Database db)
		{
			Log.Debug ("Filling object " + storable);
			SerializationContext context = new SerializationContext (db, storable.GetType ());
			Document doc = db.GetExistingDocument (storable.ID.ToString ());
			JsonSerializer serializer = GetSerializer (storable.GetType (), context, doc.CurrentRevision);
			serializer.ContractResolver = new StorablesStackContractResolver (context, storable);
			DeserializeObject (doc, storable.GetType (), context, serializer);
		}


		public static T DeserializeFromJson<T> (string json, Database db, Revision rev)
		{
			JsonSerializerSettings settings = GetSerializerSettings (typeof(T),
				                                  new SerializationContext (db, typeof(T)), rev);
			return JsonConvert.DeserializeObject<T> (json, settings);
		}

		/// <summary>
		/// Serializes an object into a <c>JObject</c>.
		/// </summary>
		/// <returns>A new object serialized.</returns>
		/// <param name="obj">The <c>IStorable</c> to serialize.</param>
		/// <param name="rev">The document revision to serialize.</param>
		/// <param name="context">The serialization context"/>
		internal static JObject SerializeObject (IStorable obj, Revision rev, SerializationContext context)
		{
			JObject jo = JObject.FromObject (obj, GetSerializer (obj.GetType (), context, rev));
			jo [DOC_TYPE] = obj.GetType ().Name;
			jo [OBJ_TYPE] = jo ["$type"];
			jo.Remove ("$type");
			return jo;
		}

		/// <summary>
		/// Deserializes a <c>Document</c>
		/// </summary>
		/// <returns>A new object deserialized.</returns>
		/// <param name="doc">The document to deserialize.</param>
		/// <param name = "objType"><see cref="Type"/> of the object to deserialize</param>
		/// <param name="context">The serialization context"/>
		internal static object DeserializeObject (Document doc, Type objType,
			SerializationContext context, JsonSerializer serializer = null)
		{
			if (serializer == null) {
				serializer = GetSerializer (objType, context, doc.CurrentRevision); 
				serializer.ContractResolver = new StorablesStackContractResolver (context, null);
			}
			JObject jo = JObject.FromObject (doc.Properties);
			return jo.ToObject (objType, serializer);
		}

		internal static JsonSerializerSettings GetSerializerSettings (Type objType,
		                                                              SerializationContext context, Revision rev)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings ();
			settings.Formatting = Formatting.Indented;
			settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
			settings.TypeNameHandling = TypeNameHandling.Objects;
			settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
			settings.Converters.Add (new ImageConverter (rev));
			settings.Converters.Add (new VersionConverter ());
			settings.Converters.Add (new LongoMatchConverter (false));
			settings.Converters.Add (new StorablesConverter (objType, context));
			return settings;
		}

		internal static JsonSerializer GetSerializer (Type objType, SerializationContext context, Revision rev)
		{
			return JsonSerializer.Create (GetSerializerSettings (objType, context, rev));
		}
	}

	/// <summary>
	/// Converts fields with <see cref="LongoMatch.Core.Common.Image"/> objects 
	/// into Attachments, using as field value the name of the attachment prefixed
	/// with the <c>attachment::</c> string.
	/// In the desrialization process, it loads <see cref="LongoMatch.Core.Common.Image"/>
	/// from the attachment with the same as the set in the property.
	/// </summary>
	class ImageConverter : JsonConverter
	{
		Revision rev;
		const string ATTACHMENT = "attachment::";
		Dictionary<string, int> attachmentNamesCount;

		public ImageConverter (Revision rev)
		{
			this.rev = rev;
			attachmentNamesCount = new Dictionary<string, int> ();
		}

		string GetAttachmentName (JsonWriter writer)
		{
			string propertyName;
			if (writer.WriteState == WriteState.Array) {
				propertyName = ((writer as JTokenWriter).Token.Last as JProperty).Name;
			} else {
				propertyName = writer.Path;
			}
			if (!attachmentNamesCount.ContainsKey (propertyName)) {
				attachmentNamesCount [propertyName] = 0;
			}
			attachmentNamesCount [propertyName]++;
			return string.Format ("{0}_{1}", propertyName, attachmentNamesCount [propertyName]);
		}

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			string attachName = GetAttachmentName (writer);
			(rev as UnsavedRevision).SetAttachment (attachName, "image/png",
				(value as Image).Serialize ());
			writer.WriteValue (ATTACHMENT + attachName);
		}

		public override object ReadJson (JsonReader reader, Type objectType,
		                                 object existingValue, JsonSerializer serializer)
		{
			if (objectType == typeof(Image)) {
				string valueString = reader.Value as string;

				if (valueString == null) {
					return null;
				}
				if (valueString.StartsWith (ATTACHMENT)) {
					string attachmentName = valueString.Replace (ATTACHMENT, "");
					Attachment attachment = rev.GetAttachment (attachmentName);
					if (attachment == null) {
						return null;
					}
					return Image.Deserialize (attachment.Content.ToArray ());
				} else {
					throw new InvalidCastException ();
				}
			}
			return reader.Value;
		}

		public override bool CanConvert (Type objectType)
		{
			return objectType.Equals (typeof(Image));
		}
	}



	/// <summary>
	/// Serializes and desrializes IStorable objects by ID using a new Document for
	/// each IStorable object.
	/// </summary>
	public class StorablesConverter : JsonConverter
	{
		SerializationContext context;
		Type objType;

		public StorablesConverter (Type objType, SerializationContext context)
		{
			this.context = context;
			this.objType = objType;
		}

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			IStorable storable = value as IStorable;

			if (!context.Cache.IsCached (storable.ID)) {
				DocumentsSerializer.SaveObject (storable, context.DB, context);
				context.Cache.AddReference (storable);
			}
			writer.WriteValue (storable.ID);
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue,
		                                 JsonSerializer serializer)
		{
			Guid id;
			IStorable storable;

			id = Guid.Parse (reader.Value as string);
			/* Return the cached object instance instead a new one */
			storable = context.Cache.ResolveReference (id);
			if (storable == null) {
				storable = DocumentsSerializer.LoadObject (objectType, id, context.DB, context) as IStorable;
				context.Cache.AddReference (storable);
			}
			return storable;
		}

		public override bool CanConvert (Type objectType)
		{
			if (typeof(IStorable).IsAssignableFrom (objectType)) {
				if (objectType != objType)
					return true;
				else
					return false;
			}
			return false;
		}
	}


	/// <summary>
	/// This custom <see cref="IContractResolver"/> is used for the following purposes:
	/// <list type="bullet">
	/// <item>
	/// <description>Create a stack of deserialized <see cref="IStorable"/> objects</description>
	/// </item>
	/// <item>
	/// <description>Re-use a partial <see cref="IStorable"/> to fill it instead of creating a new instance.</description>
	/// </item>
	/// </list>
	/// 
	/// The stack is updated overriding the default contructor to push the new object and adding
	/// a deserialized callback to pop it.
	/// 
	/// When a storable is provided in the constructor, each time a new object of the same type is created this
	/// storable is used instead of creating a new one. It's used to fill partial <see cref="IStorable"/> objects
	/// calling <see cref="IStorage.Fill"/>, assuming they haven't children with the same type.
	/// </summary>
	public class StorablesStackContractResolver : DefaultContractResolver
	{
		SerializationContext context;
		IStorable parentStorable;

		/// <summary>
		/// Initializes a new instance of the <see cref="LongoMatch.DB.StorablesStackContractResolver"/> class.
		/// If <paramref name="parentStorable"/> is not null, this storable will be used instead of creating
		/// a new instance.
		/// </summary>
		/// <param name="context">The serialization context.</param>
		/// <param name="parentStorable">The partially loaded storable that is going to be filled.</param>
		public StorablesStackContractResolver (SerializationContext context, IStorable parentStorable) {
			this.context = context;
			this.parentStorable = parentStorable;
		}

		protected override JsonContract CreateContract (Type type)
		{
			JsonContract contract = base.CreateContract(type);
			if (typeof(IStorable).IsAssignableFrom (type)) {
				contract.OnDeserializedCallbacks.Add (
					(o, context) => this.context.Stack.Pop ());
				if (parentStorable != null && type == parentStorable.GetType ()) {
					contract.DefaultCreator = () =>	{
						context.Stack.Push (parentStorable);
						parentStorable.SavedChildren = new List<IStorable> ();
						return parentStorable;
					};
				} else {
					var defaultCreator = contract.DefaultCreator;
					contract.DefaultCreator = () => {
						IStorable storable = defaultCreator () as IStorable;
						storable.SavedChildren = new List<IStorable> ();
						context.Stack.Push (storable);
						return storable;
					};
				}
			}
			return contract;
		}
	}
}

