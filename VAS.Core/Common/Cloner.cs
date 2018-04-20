// 
//  Copyright (C) 2011 Andoni Morales Alastruey
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
using VAS.Core.Interfaces;
using VAS.Core.MVVMC;
using VAS.Core.Serialization;
using VAS.Core.Store;

namespace VAS.Core.Common
{
	public static class Cloner
	{
		public static T Clone<T> (this T source, SerializationType type = SerializationType.Json)
		{
			IStorable storable = source as IStorable;
			T retStorable;
			bool storableLoaded = false;

			if (Object.ReferenceEquals (source, null)) {
				return default (T);
			}

			if (storable != null) {
				storableLoaded = storable.IsLoaded;
				storable.IsLoaded = true;
			}

			// Binary deserialization fails in mobile platforms because of
			// https://bugzilla.xamarin.com/show_bug.cgi?id=37300
#if OSTYPE_ANDROID || OSTYPE_IOS
			type = SerializationType.Json;
#endif

			if (source is BindableBase) {
				type = SerializationType.Json;
			}

			retStorable = Serializer.Instance.Clone (source, type);
			if (storable != null) {
				(retStorable as IStorable).Storage = storable.Storage;
				(retStorable as IStorable).IsLoaded = storableLoaded;
				storable.IsLoaded = storableLoaded;
			}
			return retStorable;
		}
	}
}
