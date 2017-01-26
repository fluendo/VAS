//
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace VAS.Core.License
{
	/// <summary>
	/// License limitations.
	/// </summary>
	public class LicenseLimitations<T> : ILicenseLimitations<T>
		where T : LicenseLimitation
	{
		protected Dictionary<string, T> Limitations { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VAS.Core.License.LicenseLimitations`1"/> class.
		/// </summary>
		public LicenseLimitations ()
		{
			Limitations = new Dictionary<string, T> ();
		}

		public void AddLimitation (T limitation)
		{
			Limitations [limitation.LimitationName] = limitation;
		}

		public IEnumerable<T> GetLimitations ()
		{
			return Limitations.Values;
		}

		public T GetLimitation (string limitationName)
		{
			T value;
			Limitations.TryGetValue (limitationName, out value);
			return value;
		}

		public void SetLimitationsStatus (bool status)
		{
			foreach (var limitation in GetLimitations ()) {
				limitation.Enabled = status;
			}
		}

		public void SetLimitationStatus (string limitationName, bool status)
		{
			var limit = GetLimitation (limitationName);
			if (limit != null) {
				limit.Enabled = status;
			}
		}
	}
}
