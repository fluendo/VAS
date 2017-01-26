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
using NUnit.Framework;
using VAS.Core.License;
using VAS.Core.Store;
using VAS.Core.Store.Templates;

namespace VAS.Tests.Core.License
{
	[TestFixture ()]
	public class TestLicenseLimitations
	{
		LicenseLimitations<LicenseLimitation> limitations;
		LicenseLimitation limitationPlayers;
		LicenseLimitation limitationPlayers2;
		LicenseLimitation limitationTeams;

		[TestFixtureSetUp]
		public void Setup ()
		{
			limitations = new LicenseLimitations<LicenseLimitation> ();
			limitationPlayers = new LicenseLimitation { Enabled = true, Maximum = 10, LimitationName = "RAPlayers" };
			limitationPlayers2 = new LicenseLimitation { Enabled = true, Maximum = 20, LimitationName = "RAPlayers" };
			limitationTeams = new LicenseLimitation { Enabled = true, Maximum = 5, LimitationName = "Teams" };
		}

		[Test ()]
		public void TestAddLimitations ()
		{
			limitations.AddLimitation (limitationPlayers);
			limitations.AddLimitation (limitationPlayers2);
			limitations.AddLimitation (limitationTeams);

			LicenseLimitation testLimitationPlayers = limitations.GetLimitation ("RAPlayers");
			LicenseLimitation testLimitationTeams = limitations.GetLimitation ("Teams");
			List<LicenseLimitation> allLlimitations =
				new List<LicenseLimitation> (limitations.GetLimitations ());

			Assert.AreEqual (2, allLlimitations.Count);
			Assert.IsTrue (testLimitationPlayers.Enabled);
			Assert.AreEqual (20, testLimitationPlayers.Maximum);
			Assert.IsTrue (testLimitationTeams.Enabled);
			Assert.AreEqual (5, testLimitationTeams.Maximum);
			Assert.AreEqual (2, allLlimitations.Count);
		}
	}
}
