﻿//
//  Copyright (C) 2016 Fluendo S.A.
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
using VAS.Core.MVVMC;
using VAS.Core.Store;

namespace VAS.Services.ViewModel
{
	/// <summary>
	/// Timeline event ViewModel Generic Base class
	/// </summary>
	public class TimelineEventVM<T> : ViewModelBase<T>
		where T : TimelineEvent
	{
		/// <summary>
		/// Gets or sets the Name of the TimelineEvent.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return Model.Name;
			}
			set {
				Model.Name = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance has drawings.
		/// </summary>
		/// <value><c>true</c> if this instance has drawings; otherwise, <c>false</c>.</value>
		public string HasDrawings {
			get {
				return Model.HasDrawings ? "D" : "0";
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance has field position.
		/// </summary>
		/// <value><c>true</c> if this instance has field position; otherwise, <c>false</c>.</value>
		public string HasFieldPosition {
			get {
				return (Model.FieldPosition != null) ? "X" : "0";
			}
		}

		/// <summary>
		/// Gets the notes.
		/// </summary>
		/// <value>The notes.</value>
		public string Notes {
			get {
				return (!string.IsNullOrEmpty (Model.Notes)) ? Model.Notes : "...";
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="VAS.Services.ViewModel.TimelineEventVM`1"/> is playing.
		/// </summary>
		/// <value><c>true</c> if playing; otherwise, <c>false</c>.</value>
		public bool Playing {
			get {
				return Model.Playing;
			}
			set {
				Model.Playing = value;
			}
		}
	}
}

