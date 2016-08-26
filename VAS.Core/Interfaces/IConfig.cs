// 
//  Copyright (C) 2011 Andoni Morales Alastruey
// 
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General  License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General  License for more details.
//  
//  You should have received a copy of the GNU General  License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
// 
using System;
using VAS.Core.Common;

namespace VAS.Core.Interfaces
{
	public interface IConfig
	{
		void Save ();

		bool IsChanged {
			get;
			set;
		}

		string Lang {
			get;
			set;
		}

		VideoStandard CaptureVideoStandard {
			get;
			set;
		}

		EncodingProfile CaptureEncodingProfile {
			get;
			set;
		}

		EncodingQuality CaptureEncodingQuality {
			get;
			set;
		}

		bool AutoSave {
			get;
			set;
		}

		bool AutoRenderPlaysInLive {
			get;
			set;
		}

		string AutoRenderDir {
			get;
			set;
		}

		string LastDir {
			get;
			set;
		}

		string LastRenderDir {
			get;
			set;
		}

		bool ReviewPlaysInSameWindow {
			get;
			set;
		}

		string DefaultTemplate {
			get;
			set;
		}


		ProjectSortMethod ProjectSortMethod {
			get;
			set;
		}

		Version IgnoreUpdaterVersion {
			get;
			set;
		}

		VideoStandard RenderVideoStandard {
			get;
			set;
		}

		EncodingProfile RenderEncodingProfile {
			get;
			set;
		}

		EncodingQuality RenderEncodingQuality {
			get;
			set;
		}

		bool OverlayTitle {
			get;
			set;
		}

		bool EnableAudio {
			get;
			set;
		}

		uint FPS_N {
			get;
			set;
		}

		uint FPS_D {
			get;
			set;
		}

		bool FastTagging {
			get;
			set;
		}

		string CurrentDatabase {
			get;
			set;
		}
	}

}
