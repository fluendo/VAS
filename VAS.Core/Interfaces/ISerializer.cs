﻿//
//  Copyright (C) 2015 vguzman
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
using System.IO;
using VAS.Core.Common;

namespace VAS.Core.Interfaces
{
	public interface ISerializer
	{
		void Save<T> (T obj, Stream stream,
					  SerializationType type = SerializationType.Json);

		void Save<T> (T obj, string filepath,
					  SerializationType type = SerializationType.Json);

		T Load<T> (Stream stream,
				   SerializationType type = SerializationType.Json);

		T Load<T> (string filepath,
				   SerializationType type = SerializationType.Json);

		T LoadSafe<T> (string filepath);

		/// <summary>
		/// Deep clone of object using Json and avoiding JsonIgnore
		/// </summary>
		/// <returns>The object's deep clone.</returns>
		/// <param name="obj">The object to be cloned.</param>
		/// <param name="serType">The serialization type to do the clone operation.</param>
		/// <typeparam name="T">The type of the object to be cloned.</typeparam>
		T Clone<T> (T obj, SerializationType serType = SerializationType.Json);
	}
}
