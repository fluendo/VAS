//
//  Copyright (C) 2014 Andoni Morales Alastruey
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
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using VAS.Core.Common;

namespace VAS.Core.Store
{
	[Serializable]
	[PropertyChanged.ImplementPropertyChanged]
	public class Timer : StorableBase
	{
		public Timer ()
		{
			Nodes = new RangeObservableCollection<TimeNode> ();
			ID = Guid.NewGuid ();
		}

		public virtual string Name {
			get;
			set;
		}

		public RangeObservableCollection<TimeNode> Nodes {
			get;
			set;
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public Time TotalTime {
			get {
				return new Time (Nodes.Where (tn => tn.Start != null && tn.Stop != null)
					.Sum (tn => tn.Duration.MSeconds));
			}
		}

		public TimeNode Start (Time start, string name = null)
		{
			TimeNode tn;

			if (name == null)
				name = Name;
			Stop (start);
			tn = new TimeNode { Name = name, Start = start };
			Nodes.Add (tn);
			return tn;
		}

		public void Stop (Time stop)
		{
			if (Nodes.Count > 0) {
				TimeNode last = Nodes.Last ();
				if (last.Stop == null) {
					last.Stop = stop;
				}
			}
			Nodes.OrderBy (tn => tn.Start.MSeconds);
		}

		public void CancelCurrent ()
		{
			if (Nodes.Count > 0) {
				TimeNode last = Nodes.Last ();
				if (last.Stop == null) {
					Nodes.Remove (last);
				}
			}
		}
	}
}

