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
using Cairo;
using VAS.Core.Common;
using VAS.Core.Interfaces.Drawing;

namespace VAS.Drawing.Cairo
{
	public class Surface: ISurface
	{
		ImageSurface surface;
		bool disposed, warnOnDispose;

		public Surface (int width, int height, Image image, bool warnOnDispose = true)
		{
			this.warnOnDispose = warnOnDispose;
			surface = new ImageSurface (Format.ARGB32, width, height);
			if (image != null) {
				using (Context context = new Context (surface)) {
					Gdk.CairoHelper.SetSourcePixbuf (context, image.Value, 0, 0);
					context.Paint ();
				}
			}
		}

		~Surface ()
		{
			if (!disposed && warnOnDispose) {
				Log.Error (String.Format ("Surface {0} was not disposed correctly", this));
				Dispose (true);
			}
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (disposing) {
				if (surface != null) {
					surface.Dispose ();
				}
				surface = null;
				disposed = true;
			}
		}

		public Surface (string filename)
		{
			surface = new ImageSurface (filename);
		}

		public object Value {
			get {
				return surface;
			}
		}

		public IContext Context {
			get {
				return new CairoContext (surface);
			}
		}

		public int Width {
			get {
				return surface.Width;
			}
		}

		public int Height {
			get {
				return surface.Height;
			}
		}

		public Image Copy ()
		{
			Gdk.Colormap colormap = Gdk.Colormap.System;

			/* FIXME: copying a surface to a pixbuf through a pixmap does not require writting / reading to a file,
			 * but it does not handle transparencies correctly and draws transparent pixels in a black color, so
			 * for now we force the first method */
			colormap = null;

			/* In unit tests running without a display, the default Screen is null and we can't get a valid colormap.
			 * In this scenario we use a fallback that writes the surface to a temporary file */
			if (colormap == null) {
				string tempFile = System.IO.Path.GetTempFileName ();
				surface.WriteToPng (tempFile);
				Gdk.Pixbuf pixbuf = new Gdk.Pixbuf (tempFile);
				return new Image (pixbuf);
			} else {
				Gdk.Pixmap pixmap = new Gdk.Pixmap (null, Width, Height, 24);
				using (Context cr = Gdk.CairoHelper.Create (pixmap)) {
					cr.Operator = Operator.Source;
					cr.SetSource (surface);
					cr.Paint ();
				}
				return new Image (Gdk.Pixbuf.FromDrawable (pixmap, Gdk.Colormap.System, 0, 0, 0, 0, Width, Height));
			}
		}
	}
}

