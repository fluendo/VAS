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
using Gtk;
using VAS.Core;
using VAS.Core.Common;
using VAS.Core.Store.Playlists;
using VASMisc = VAS.UI.Helpers.Misc;

namespace VAS.UI.Dialog
{
	[System.ComponentModel.Category ("VAS")]
	[System.ComponentModel.ToolboxItem (false)]
	public partial class VideoEditionProperties : Gtk.Dialog
	{
		EncodingSettings encSettings;
		ListStore stdStore, encStore, qualStore;

		#region Constructors

		public VideoEditionProperties (Window parent)
		{
			TransientFor = parent;
			this.Build ();
			encSettings = new EncodingSettings ();
			stdStore = VASMisc.FillImageFormat (sizecombobox, VideoStandards.Rendering,
				App.Current.Config.RenderVideoStandard);
			encStore = VASMisc.FillEncodingFormat (formatcombobox, App.Current.Config.RenderEncodingProfile);
			qualStore = VASMisc.FillQuality (qualitycombobox, App.Current.Config.RenderEncodingQuality);
			descriptioncheckbutton.Active = App.Current.Config.OverlayTitle;
			audiocheckbutton.Active = App.Current.Config.EnableAudio;
			mediafilechooser1.FileChooserMode = FileChooserMode.File;
			mediafilechooser1.FilterName = "Multimedia Files";
			mediafilechooser1.FilterExtensions = new string [] {"*.mkv", "*.mp4", "*.ogg",
				"*.avi", "*.mpg", "*.vob"
			};
			mediafilechooser2.FileChooserMode = FileChooserMode.Directory;
			mediafilechooser2.ChangedEvent += (sender, e) => {
				OutputDir = mediafilechooser2.CurrentPath;
			};
		}

		#endregion

		#region Properties

		public EncodingSettings EncodingSettings {
			get {
				return encSettings;
			}
		}

		public String OutputDir {
			get;
			set;
		}

		public bool SplitFiles {
			get;
			set;
		}

		public Playlist Playlist {
			set {
				if (value.Name != null) {
					mediafilechooser1.ProposedFileName = value.Name + ".mp4";
					mediafilechooser1.ProposedDirectoryName = value.Name;
				}
			}
		}

		#endregion Properties

		#region Private Methods

		string GetExtension ()
		{
			TreeIter iter;
			formatcombobox.GetActiveIter (out iter);
			return ((EncodingProfile)encStore.GetValue (iter, 1)).Extension;
		}

		#endregion

		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			TreeIter iter;

			/* Get size info */
			sizecombobox.GetActiveIter (out iter);
			encSettings.VideoStandard = (VideoStandard)stdStore.GetValue (iter, 1);

			/* Get encoding profile info */
			formatcombobox.GetActiveIter (out iter);
			encSettings.EncodingProfile = (EncodingProfile)encStore.GetValue (iter, 1);

			/* Get quality info */
			qualitycombobox.GetActiveIter (out iter);
			encSettings.EncodingQuality = (EncodingQuality)qualStore.GetValue (iter, 1);

			encSettings.OutputFile = mediafilechooser1.CurrentPath;

			encSettings.Framerate_n = App.Current.Config.FPS_N;
			encSettings.Framerate_d = App.Current.Config.FPS_D;

			encSettings.TitleSize = 20;

			encSettings.EnableAudio = audiocheckbutton.Active;
			encSettings.EnableTitle = descriptioncheckbutton.Active;

			if (!SplitFiles && String.IsNullOrEmpty (EncodingSettings.OutputFile)) {
				App.Current.Dialogs.WarningMessage (Catalog.GetString ("Please, select a video file."));
			} else if (SplitFiles && String.IsNullOrEmpty (OutputDir)) {
				App.Current.Dialogs.WarningMessage (Catalog.GetString ("Please, select an output directory."));
			} else {
				Respond (ResponseType.Ok);
			}
		}

		protected void OnSplitfilesbuttonClicked (object sender, System.EventArgs e)
		{
			dirbox.Visible = splitfilesbutton.Active;
			filebox.Visible = !splitfilesbutton.Active;
			SplitFiles = splitfilesbutton.Active;
		}
	}
}
