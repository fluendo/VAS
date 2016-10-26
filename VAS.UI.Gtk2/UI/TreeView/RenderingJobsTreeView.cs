//
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

using Gtk;
using Stetic;
using VAS.Core;
using VAS.Core.Common;
using VAS.Core.Services.ViewModel;
using VAS.Services.ViewModel;
using VAS.UI.Common;

namespace VAS.UI.TreeView
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class RenderingJobsTreeView : TreeViewBase<JobCollectionVM, Job, JobVM>
	{
		public RenderingJobsTreeView ()
		{
			TreeViewColumn nameColumn = new TreeViewColumn ();
			nameColumn.Title = Catalog.GetString ("Job name");
			CellRendererText nameCell = new CellRendererText ();
			nameColumn.PackStart (nameCell, true);

			TreeViewColumn stateColumn = new TreeViewColumn ();
			stateColumn.Title = Catalog.GetString ("State");
			CellRendererPixbuf stateCell = new CellRendererPixbuf ();
			stateColumn.PackStart (stateCell, true);

			nameColumn.SetCellDataFunc (nameCell, new Gtk.TreeCellDataFunc (RenderName));
			stateColumn.SetCellDataFunc (stateCell, new Gtk.TreeCellDataFunc (RenderState));

			AppendColumn (nameColumn);
			AppendColumn (stateColumn);
		}

		private void RenderName (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			JobVM jobVM = (JobVM)model.GetValue (iter, 0) as JobVM;
			(cell as Gtk.CellRendererText).Text = jobVM.Name;
		}

		private void RenderState (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			JobVM jobVM = model.GetValue (iter, 0) as JobVM;
			(cell as Gtk.CellRendererPixbuf).Pixbuf = IconLoader.LoadIcon (this, jobVM.StateIconName, IconSize.Button);
		}

		protected override void HandleViewModelPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.HandleViewModelPropertyChanged (sender, e);
			QueueDraw ();
		}
	}
}
