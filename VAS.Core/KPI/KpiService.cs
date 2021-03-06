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
//
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.HockeyApp;
using VAS.Core.Interfaces;

namespace VAS.KPI
{
	public class KpiService : IKpiService
	{
		bool initialized = false;

		/// <summary>
		/// Gets the user identifier.
		/// </summary>
		/// <value>The user identifier.</value>
		public string UserID {
			get {
				return HockeyClient.Current.AsInternal ().UserID;
			}
		}

		/// <summary>
		/// Gets the session identifier.
		/// </summary>
		/// <value>The session identifier.</value>
		public string SessionID {
			get {
				return HockeyClient.Current.AsInternal ().SessionID;
			}
		}

		public async Task Init (string appId, string user, string email)
		{
#if !DEBUG
			(await HockeyClient.Current.Configure (appId))
						.SetContactInfo (user, email);
			await HockeyClient.Current.SendCrashesAsync ();
			TrackEvent ("Session_Start", null, null);
			initialized = true;
#endif
		}

		public void TrackEvent (string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
		{
#if !DEBUG
			if (!initialized) return;
			HockeyClient.Current.TrackEvent (eventName, properties, metrics);
#endif
		}

		public void TrackException (Exception ex, IDictionary<string, string> properties)
		{
#if !DEBUG
			if (!initialized) return;
			HockeyClient.Current.HandleException (ex);
#endif
		}

		public void Flush ()
		{
#if !DEBUG
			if (!initialized) return;
			HockeyClient.Current.Flush ();
			HockeyClient.Current.SendCrashesAsync ().ConfigureAwait (false);
#endif
		}
	}
}
