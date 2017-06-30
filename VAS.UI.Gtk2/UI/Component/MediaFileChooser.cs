﻿//
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
using VAS.Core;
using VAS.Core.Common;
using VAS.Core.Store;

namespace VAS.UI.Component
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class MediaFileChooser : Gtk.Bin
	{
		public event EventHandler ChangedEvent;
		public event EventHandler NameChangedEvent;

		MediaFile mediaFile;
		string path;
		string proposedFileName;
		string proposedDirectoryName;

		public MediaFileChooser (String name)
		{
			this.Build ();

			nameentry.NoShowAll = true;

			// The name entry is only visible when not empty
			nameentry.Visible = !String.IsNullOrEmpty (name);
			nameentry.Text = name ?? "";

			addbuttonimage.Pixbuf = Helpers.Misc.LoadIcon ("vas-browse", Gtk.IconSize.Button, 0);
			clearbuttonimage.Pixbuf = Helpers.Misc.LoadIcon ("vas-delete", Gtk.IconSize.Button, 0);

			FilterName = "MP4";
			FilterExtensions = new string [] { "*.mp4" };
			FileChooserMode = FileChooserMode.MediaFile;

			UpdateFile ();

			entrybutton_addbutton.Clicked += HandleAddClicked;
			clearbutton.Clicked += HandleClearClicked;
			nameentry.Changed += HandleNameChanged;

			nameentry.TooltipText = Catalog.GetString ("Edit to change camera name");

			ProposedFileName = String.Format ("{0}-{1}.mp4", App.Current.SoftwareName, DateTime.Now.ToShortDateString ());
			ProposedDirectoryName = String.Format ("{0}-{1}", App.Current.SoftwareName, DateTime.Now.ToShortDateString ());
		}

		public MediaFileChooser () : this (null)
		{
		}

		public FileChooserMode FileChooserMode {
			get;
			set;
		}

		public string ProposedFileName {
			get {
				return proposedFileName;
			}
			set {
				proposedFileName = Utils.SanitizePath (value);
			}
		}

		public string ProposedDirectoryName {
			get {
				return proposedDirectoryName;
			}
			set {
				proposedDirectoryName = Utils.SanitizePath (value);
			}
		}

		public string CurrentPath {
			set {
				path = value;
				UpdateFile ();
			}
			get {
				return path;
			}
		}

		public string FilterName {
			get;
			set;
		}

		public string [] FilterExtensions {
			get;
			set;
		}

		public MediaFile MediaFile {
			get {
				if (mediaFile != null && nameentry.Visible && !String.IsNullOrEmpty (nameentry.Text)) {
					mediaFile.Name = nameentry.Text;
				}
				return mediaFile;
			}
			set {
				mediaFile = value;
				UpdateFile ();
			}
		}

		void UpdateFile ()
		{
			bool clear_visible = false;

			if (mediaFile != null) {
				fileentry.Text = System.IO.Path.GetFileName (mediaFile.FilePath);
				fileentry.TooltipText = mediaFile.FilePath;
				if (mediaFile.Exists ()) {
					fileentry.ModifyText (Gtk.StateType.Normal, Helpers.Misc.ToGdkColor (App.Current.Style.PaletteText));
				} else {
					fileentry.ModifyText (Gtk.StateType.Normal, Helpers.Misc.ToGdkColor (Color.Red1));
				}
				clear_visible = true;
			} else if (path != null) {
				fileentry.Text = System.IO.Path.GetFileName (path);
				fileentry.TooltipText = path;
				clear_visible = true;
			} else {
				if (FileChooserMode == FileChooserMode.Directory) {
					fileentry.Text = Catalog.GetString ("Select folder...");
				} else {
					fileentry.Text = Catalog.GetString ("Select file...");
				}
				fileentry.TooltipText = fileentry.Text;
			}

			clearbutton.Sensitive = clear_visible;
		}

		void HandleClearClicked (object sender, EventArgs e)
		{
			mediaFile = null;
			path = null;

			UpdateFile ();

			if (ChangedEvent != null) {
				ChangedEvent (this, null);
			}
		}

		void HandleAddClicked (object sender, EventArgs e)
		{
			if (FileChooserMode == FileChooserMode.MediaFile) {
				MediaFile file = Helpers.Misc.OpenFile (this);
				if (file != null && MediaFile != null) {
					file.Offset = MediaFile.Offset;
				}
				MediaFile = file;
			} else if (FileChooserMode == FileChooserMode.File) {
				CurrentPath = Helpers.FileChooserHelper.SaveFile (this, Catalog.GetString ("Output file"),
					ProposedFileName, App.Current.Config.LastRenderDir,
					FilterName, FilterExtensions);
				if (CurrentPath != null) {
					App.Current.Config.LastRenderDir = System.IO.Path.GetDirectoryName (CurrentPath);
				}
			} else if (FileChooserMode == FileChooserMode.Directory) {
				CurrentPath = Helpers.FileChooserHelper.SelectFolder (this, Catalog.GetString ("Output folder"),
					ProposedDirectoryName, App.Current.Config.LastRenderDir,
					null, null);
			}
			if (ChangedEvent != null) {
				ChangedEvent (this, null);
			}
		}

		void HandleNameChanged (object sender, EventArgs e)
		{
			if (mediaFile != null) {
				mediaFile.Name = nameentry.Text;
			}

			if (NameChangedEvent != null) {
				NameChangedEvent (this, null);
			}
		}
	}
}
