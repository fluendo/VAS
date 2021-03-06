//  Copyright (C) 2007-2009 Andoni Morales Alastruey
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
//
//
using System;
using System.Runtime.InteropServices;
using VAS.Core.Common;
using VAS.Core.Interfaces.Multimedia;
using VAS.Multimedia.Common;
using VASHandlers = VAS.Core.Handlers;

namespace VAS.Multimedia.Remuxer
{
	#region Autogenerated code
	public  class GstRemuxer : GLib.Object, IRemuxer
	{

		public event VASHandlers.ProgressHandler Progress;
		public event VASHandlers.ErrorHandler Error;

		[DllImport ("libvas.dll")]
		static extern unsafe IntPtr gst_remuxer_new (IntPtr input_file, IntPtr output_file, VideoMuxerType muxer, out IntPtr err);

		public unsafe GstRemuxer (string inputFile, string outputFile, VideoMuxerType muxer) : base (IntPtr.Zero)
		{
			if (GetType () != typeof(GstRemuxer)) {
				throw new InvalidOperationException ("Can't override this constructor.");
			}
			IntPtr error = IntPtr.Zero;
			Raw = gst_remuxer_new (GLib.Marshaller.StringToPtrGStrdup (inputFile),
				GLib.Marshaller.StringToPtrGStrdup (outputFile),
				muxer, out error);
			if (error != IntPtr.Zero)
				throw new GLib.GException (error);
			
			PercentCompleted += delegate(object o, PercentCompletedArgs args) {
				if (Progress != null)
					Progress (args.Percent);
			};
			
			GstError += delegate(object o, ErrorArgs args) {
				if (Error != null)
					Error (this, args.Message);
			};
		}
		#pragma warning disable 0169
		#region Error

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate void ErrorSignalDelegate (IntPtr arg0,IntPtr arg1,IntPtr gch);

		static void ErrorSignalCallback (IntPtr arg0, IntPtr arg1, IntPtr gch)
		{
			ErrorArgs args = new ErrorArgs ();
			try {
				GLib.Signal sig = ((GCHandle)gch).Target as GLib.Signal;
				if (sig == null)
					throw new Exception ("Unknown signal GC handle received " + gch);

				args.Args = new object[1];
				args.Args [0] = GLib.Marshaller.Utf8PtrToString (arg1);
				GlibErrorHandler handler = (GlibErrorHandler)sig.Handler;
				handler (GLib.Object.GetObject (arg0), args);
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
		}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate void ErrorVMDelegate (IntPtr remuxer,IntPtr message);

		static ErrorVMDelegate ErrorVMCallback;

		static void error_cb (IntPtr remuxer, IntPtr message)
		{
			try {
				GstRemuxer remuxer_managed = GLib.Object.GetObject (remuxer, false) as GstRemuxer;
				remuxer_managed.OnError (GLib.Marshaller.Utf8PtrToString (message));
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
		}

		private static void OverrideError (GLib.GType gtype)
		{
			if (ErrorVMCallback == null)
				ErrorVMCallback = new ErrorVMDelegate (error_cb);
			OverrideVirtualMethod (gtype, "error", ErrorVMCallback);
		}

		[GLib.DefaultSignalHandler (Type = typeof(GstRemuxer), ConnectionMethod = "OverrideError")]
		protected virtual void OnError (string message)
		{
			GLib.Value ret = GLib.Value.Empty;
			GLib.ValueArray inst_and_params = new GLib.ValueArray (2);
			GLib.Value[] vals = new GLib.Value [2];
			vals [0] = new GLib.Value (this);
			inst_and_params.Append (vals [0]);
			vals [1] = new GLib.Value (message);
			inst_and_params.Append (vals [1]);
			g_signal_chain_from_overridden (inst_and_params.ArrayPtr, ref ret);
			foreach (GLib.Value v in vals)
				v.Dispose ();
		}

		[GLib.Signal ("error")]
		public event GlibErrorHandler GstError {
			add {
				GLib.Signal sig = GLib.Signal.Lookup (this, "error", new ErrorSignalDelegate (ErrorSignalCallback));
				sig.AddDelegate (value);
			}
			remove {
				GLib.Signal sig = GLib.Signal.Lookup (this, "error", new ErrorSignalDelegate (ErrorSignalCallback));
				sig.RemoveDelegate (value);
			}
		}

		#endregion

		#region Percent-completed

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate void PercentCompletedVMDelegate (IntPtr gvc,float percent);

		static PercentCompletedVMDelegate PercentCompletedVMCallback;

		static void percentcompleted_cb (IntPtr remuxer, float percent)
		{
			try {
				GstRemuxer remuxer_managed = GLib.Object.GetObject (remuxer, false) as GstRemuxer;
				remuxer_managed.OnPercentCompleted (percent);
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
		}

		private static void OverridePercentCompleted (GLib.GType gtype)
		{
			if (PercentCompletedVMCallback == null)
				PercentCompletedVMCallback = new PercentCompletedVMDelegate (percentcompleted_cb);
			OverrideVirtualMethod (gtype, "percent_completed", PercentCompletedVMCallback);
		}

		[GLib.DefaultSignalHandler (Type = typeof(GstRemuxer), ConnectionMethod = "OverridePercentCompleted")]
		protected virtual void OnPercentCompleted (float percent)
		{
			GLib.Value ret = GLib.Value.Empty;
			GLib.ValueArray inst_and_params = new GLib.ValueArray (2);
			GLib.Value[] vals = new GLib.Value [2];
			vals [0] = new GLib.Value (this);
			inst_and_params.Append (vals [0]);
			vals [1] = new GLib.Value (percent);
			inst_and_params.Append (vals [1]);
			g_signal_chain_from_overridden (inst_and_params.ArrayPtr, ref ret);
			foreach (GLib.Value v in vals)
				v.Dispose ();
		}

		[GLib.Signal ("percent_completed")]
		public event GlibPercentCompletedHandler PercentCompleted {
			add {
				GLib.Signal sig = GLib.Signal.Lookup (this, "percent_completed", typeof(PercentCompletedArgs));
				sig.AddDelegate (value);
			}
			remove {
				GLib.Signal sig = GLib.Signal.Lookup (this, "percent_completed", typeof(PercentCompletedArgs));
				sig.RemoveDelegate (value);
			}
		}

		#endregion

		[DllImport ("libvas.dll")]
		static extern void gst_remuxer_cancel (IntPtr raw);

		public void Cancel ()
		{
			gst_remuxer_cancel (Handle);
		}

		[DllImport ("libvas.dll")]
		static extern void gst_remuxer_start (IntPtr raw);

		public void Start ()
		{
			gst_remuxer_start (Handle);
		}

		static GstRemuxer ()
		{
			VAS.Multimedia.Remuxer.ObjectManager.Initialize ();
		}

		#endregion
	}
}
