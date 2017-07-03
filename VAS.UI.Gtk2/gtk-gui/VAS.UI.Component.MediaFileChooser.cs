
namespace VAS.UI.Component
{
	public partial class MediaFileChooser
	{
		private global::Gtk.EventBox fileentryeventbox;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Entry nameentry;

		private global::Gtk.VSeparator vseparator1;

		private global::Gtk.Entry fileentry;

		private global::Gtk.Button clearbutton;

		private global::VAS.UI.Helpers.ImageView clearbuttonimage;

		private global::Gtk.Button entrybutton_addbutton;

		private global::VAS.UI.Helpers.ImageView addbuttonimage;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget VAS.UI.Component.MediaFileChooser
			global::Stetic.BinContainer.Attach (this);
			this.Name = "VAS.UI.Component.MediaFileChooser";
			// Container child VAS.UI.Component.MediaFileChooser.Gtk.Container+ContainerChild
			this.fileentryeventbox = new global::Gtk.EventBox ();
			this.fileentryeventbox.Name = "fileentryeventbox";
			// Container child fileentryeventbox.Gtk.Container+ContainerChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.BorderWidth = ((uint)(2));
			// Container child hbox1.Gtk.Box+BoxChild
			this.nameentry = new global::Gtk.Entry ();
			this.nameentry.CanFocus = true;
			this.nameentry.Name = "nameentry";
			this.nameentry.IsEditable = true;
			this.nameentry.HasFrame = false;
			this.nameentry.InvisibleChar = '•';
			this.hbox1.Add (this.nameentry);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.nameentry]));
			w1.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vseparator1 = new global::Gtk.VSeparator ();
			this.vseparator1.Name = "vseparator1";
			this.hbox1.Add (this.vseparator1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vseparator1]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.fileentry = new global::Gtk.Entry ();
			this.fileentry.CanFocus = true;
			this.fileentry.Name = "fileentry";
			this.fileentry.IsEditable = false;
			this.fileentry.HasFrame = false;
			this.fileentry.InvisibleChar = '•';
			this.hbox1.Add (this.fileentry);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.fileentry]));
			w3.Position = 2;
			// Container child hbox1.Gtk.Box+BoxChild
			this.clearbutton = new global::Gtk.Button ();
			this.clearbutton.CanFocus = true;
			this.clearbutton.Name = "clearbutton";
			// Container child clearbutton.Gtk.Container+ContainerChild
			this.clearbuttonimage = new global::VAS.UI.Helpers.ImageView ();
			this.clearbuttonimage.Name = "clearbuttonimage";
			this.clearbutton.Add (this.clearbuttonimage);
			this.hbox1.Add (this.clearbutton);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.clearbutton]));
			w5.Position = 3;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.entrybutton_addbutton = new global::Gtk.Button ();
			this.entrybutton_addbutton.CanFocus = true;
			this.entrybutton_addbutton.Name = "entrybutton_addbutton";
			// Container child entrybutton_addbutton.Gtk.Container+ContainerChild
			this.addbuttonimage = new global::VAS.UI.Helpers.ImageView ();
			this.addbuttonimage.Name = "addbuttonimage";
			this.entrybutton_addbutton.Add (this.addbuttonimage);
			this.hbox1.Add (this.entrybutton_addbutton);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.entrybutton_addbutton]));
			w7.Position = 4;
			w7.Expand = false;
			w7.Fill = false;
			this.fileentryeventbox.Add (this.hbox1);
			this.Add (this.fileentryeventbox);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.nameentry.Hide ();
			this.Hide ();
		}
	}
}
