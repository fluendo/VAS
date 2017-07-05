
// This file has been generated by the GUI designer. Do not modify.
namespace VAS.UI
{
	public partial class VideoPlayerView
	{
		private global::Gtk.EventBox maineventbox;

		private global::Gtk.VBox totalbox;

		private global::Gtk.HBox mainbox;

		private global::Gtk.HPaned videohpaned;

		private global::Gtk.HBox videobox;

		private global::VAS.UI.VideoWindow mainviewport;

		private global::Gtk.DrawingArea blackboarddrawingarea;

		private global::Gtk.VBox subviewportsbox;

		private global::VAS.UI.SubViewport subviewport1;

		private global::VAS.UI.SubViewport subviewport2;

		private global::VAS.UI.SubViewport subviewport3;

		private global::Gtk.EventBox lightbackgroundeventbox;

		private global::Gtk.Alignment alignment1;

		private global::Gtk.VBox vbox1;

		private global::Gtk.DrawingArea timerulearea;

		private global::Gtk.HBox controlsbox;

		private global::Gtk.Alignment leftBlockAlignment;

		private global::Gtk.HBox hbox2;

		private global::Gtk.Button closebutton;

		private global::VAS.UI.Helpers.ImageView closebuttonimage;

		private global::Gtk.Button drawbutton;

		private global::VAS.UI.Helpers.ImageView drawbuttonimage;

		private global::Gtk.Label eventNameLabel;

		private global::Gtk.HBox timeHBox;

		private global::Gtk.VSeparator timeLeftSeparator;

		private global::Gtk.VBox timeVBox;

		private global::Gtk.Label timeLabel;

		private global::Gtk.Label totalTimeLabel;

		private global::Gtk.VSeparator timeRightSeparator;

		private global::Gtk.HBox hbox4;

		private global::Gtk.Alignment centerBlockAlignment;

		private global::Gtk.HBox buttonsbox;

		private global::Gtk.Button prevbutton;

		private global::VAS.UI.Helpers.ImageView prevbuttonimage;

		private global::Gtk.Button playbutton;

		private global::VAS.UI.Helpers.ImageView playbuttonimage;

		private global::Gtk.Button pausebutton;

		private global::VAS.UI.Helpers.ImageView pausebuttonimage;

		private global::Gtk.Button nextbutton;

		private global::VAS.UI.Helpers.ImageView nextbuttonimage;

		private global::Gtk.Alignment rightBlockAlignment;

		private global::Gtk.HBox hbox3;

		private global::Gtk.HBox jumpsbox;

		private global::Gtk.VSeparator jumpsLeftSeparator;

		private global::Gtk.VBox vbox3;

		private global::Gtk.Button jumpsButton;

		private global::VAS.UI.Helpers.ImageView jumpsButtonImage;

		private global::Gtk.Label jumpsLabel;

		private global::Gtk.VSeparator jumpsRightSeparator;

		private global::Gtk.VBox zoomBox;

		private global::Gtk.Button zoomLevelButton;

		private global::VAS.UI.Helpers.ImageView zoomLevelImage;

		private global::Gtk.Label zoomLabel;

		private global::Gtk.VSeparator zoomSeparator;

		private global::Gtk.VBox rateBox;

		private global::Gtk.Button rateLevelButton;

		private global::VAS.UI.Helpers.ImageView rateLevelButtonImage;

		private global::Gtk.Label rateLabel;

		private global::Gtk.VBox durationBox;

		private global::Gtk.ToggleButton DurationButton;

		private global::VAS.UI.Helpers.ImageView DurationButtonImage;

		private global::Gtk.Label durationLabel;

		private global::Gtk.VSeparator durationSeparator;

		private global::Gtk.HBox viewportsBox;

		private global::Gtk.ToggleButton viewportsSwitchButton;

		private global::VAS.UI.Helpers.ImageView viewportsSwitchImage;

		private global::Gtk.VSeparator viewportsSeparator;

		private global::Gtk.HBox center_playhead_box;

		private global::Gtk.Button centerplayheadbutton;

		private global::VAS.UI.Helpers.ImageView centerplayheadbuttonimage;

		private global::Gtk.VSeparator centerPlayHeadSeparator;

		private global::Gtk.Button detachbutton;

		private global::VAS.UI.Helpers.ImageView detachbuttonimage;

		private global::Gtk.Button volumebutton;

		private global::VAS.UI.Helpers.ImageView volumebuttonimage;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget VAS.UI.VideoPlayerView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "VAS.UI.VideoPlayerView";
			// Container child VAS.UI.VideoPlayerView.Gtk.Container+ContainerChild
			this.maineventbox = new global::Gtk.EventBox();
			this.maineventbox.Name = "maineventbox";
			// Container child maineventbox.Gtk.Container+ContainerChild
			this.totalbox = new global::Gtk.VBox();
			this.totalbox.Name = "totalbox";
			// Container child totalbox.Gtk.Box+BoxChild
			this.mainbox = new global::Gtk.HBox();
			this.mainbox.Name = "mainbox";
			// Container child mainbox.Gtk.Box+BoxChild
			this.videohpaned = new global::Gtk.HPaned();
			this.videohpaned.CanFocus = true;
			this.videohpaned.Name = "videohpaned";
			this.videohpaned.Position = 883;
			// Container child videohpaned.Gtk.Paned+PanedChild
			this.videobox = new global::Gtk.HBox();
			this.videobox.WidthRequest = 200;
			this.videobox.Name = "videobox";
			// Container child videobox.Gtk.Box+BoxChild
			this.mainviewport = new global::VAS.UI.VideoWindow();
			this.mainviewport.Events = ((global::Gdk.EventMask)(256));
			this.mainviewport.Name = "mainviewport";
			this.mainviewport.Ratio = 1F;
			this.videobox.Add(this.mainviewport);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.videobox[this.mainviewport]));
			w1.Position = 0;
			// Container child videobox.Gtk.Box+BoxChild
			this.blackboarddrawingarea = new global::Gtk.DrawingArea();
			this.blackboarddrawingarea.Name = "blackboarddrawingarea";
			this.videobox.Add(this.blackboarddrawingarea);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.videobox[this.blackboarddrawingarea]));
			w2.Position = 1;
			this.videohpaned.Add(this.videobox);
			global::Gtk.Paned.PanedChild w3 = ((global::Gtk.Paned.PanedChild)(this.videohpaned[this.videobox]));
			w3.Resize = false;
			w3.Shrink = false;
			// Container child videohpaned.Gtk.Paned+PanedChild
			this.subviewportsbox = new global::Gtk.VBox();
			this.subviewportsbox.WidthRequest = 150;
			this.subviewportsbox.Name = "subviewportsbox";
			// Container child subviewportsbox.Gtk.Box+BoxChild
			this.subviewport1 = new global::VAS.UI.SubViewport();
			this.subviewport1.Events = ((global::Gdk.EventMask)(256));
			this.subviewport1.Name = "subviewport1";
			this.subviewport1.Index = 0;
			this.subviewportsbox.Add(this.subviewport1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.subviewportsbox[this.subviewport1]));
			w4.Position = 0;
			// Container child subviewportsbox.Gtk.Box+BoxChild
			this.subviewport2 = new global::VAS.UI.SubViewport();
			this.subviewport2.Events = ((global::Gdk.EventMask)(256));
			this.subviewport2.Name = "subviewport2";
			this.subviewport2.Index = 0;
			this.subviewportsbox.Add(this.subviewport2);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.subviewportsbox[this.subviewport2]));
			w5.Position = 1;
			// Container child subviewportsbox.Gtk.Box+BoxChild
			this.subviewport3 = new global::VAS.UI.SubViewport();
			this.subviewport3.Events = ((global::Gdk.EventMask)(256));
			this.subviewport3.Name = "subviewport3";
			this.subviewport3.Index = 0;
			this.subviewportsbox.Add(this.subviewport3);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.subviewportsbox[this.subviewport3]));
			w6.Position = 2;
			this.videohpaned.Add(this.subviewportsbox);
			global::Gtk.Paned.PanedChild w7 = ((global::Gtk.Paned.PanedChild)(this.videohpaned[this.subviewportsbox]));
			w7.Resize = false;
			w7.Shrink = false;
			this.mainbox.Add(this.videohpaned);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.mainbox[this.videohpaned]));
			w8.Position = 0;
			this.totalbox.Add(this.mainbox);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.totalbox[this.mainbox]));
			w9.Position = 0;
			// Container child totalbox.Gtk.Box+BoxChild
			this.lightbackgroundeventbox = new global::Gtk.EventBox();
			this.lightbackgroundeventbox.Name = "lightbackgroundeventbox";
			// Container child lightbackgroundeventbox.Gtk.Container+ContainerChild
			this.alignment1 = new global::Gtk.Alignment(0F, 0.5F, 1F, 1F);
			this.alignment1.Name = "alignment1";
			// Container child alignment1.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.timerulearea = new global::Gtk.DrawingArea();
			this.timerulearea.Name = "timerulearea";
			this.vbox1.Add(this.timerulearea);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.timerulearea]));
			w10.Position = 0;
			w10.Expand = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.controlsbox = new global::Gtk.HBox();
			this.controlsbox.Name = "controlsbox";
			this.controlsbox.Spacing = 6;
			// Container child controlsbox.Gtk.Box+BoxChild
			this.leftBlockAlignment = new global::Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
			this.leftBlockAlignment.Name = "leftBlockAlignment";
			// Container child leftBlockAlignment.Gtk.Container+ContainerChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.closebutton = new global::Gtk.Button();
			this.closebutton.TooltipMarkup = "Close loaded event";
			this.closebutton.Name = "closebutton";
			this.closebutton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child closebutton.Gtk.Container+ContainerChild
			this.closebuttonimage = new global::VAS.UI.Helpers.ImageView();
			this.closebuttonimage.Name = "closebuttonimage";
			this.closebutton.Add(this.closebuttonimage);
			this.hbox2.Add(this.closebutton);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.closebutton]));
			w12.Position = 0;
			w12.Expand = false;
			w12.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.drawbutton = new global::Gtk.Button();
			this.drawbutton.TooltipMarkup = "Draw frame";
			this.drawbutton.Name = "drawbutton";
			this.drawbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child drawbutton.Gtk.Container+ContainerChild
			this.drawbuttonimage = new global::VAS.UI.Helpers.ImageView();
			this.drawbuttonimage.Name = "drawbuttonimage";
			this.drawbutton.Add(this.drawbuttonimage);
			this.hbox2.Add(this.drawbutton);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.drawbutton]));
			w14.Position = 1;
			w14.Expand = false;
			w14.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.eventNameLabel = new global::Gtk.Label();
			this.eventNameLabel.WidthRequest = 0;
			this.eventNameLabel.Name = "eventNameLabel";
			this.hbox2.Add(this.eventNameLabel);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.eventNameLabel]));
			w15.Position = 2;
			w15.Expand = false;
			w15.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.timeHBox = new global::Gtk.HBox();
			this.timeHBox.Name = "timeHBox";
			this.timeHBox.Spacing = 6;
			// Container child timeHBox.Gtk.Box+BoxChild
			this.timeLeftSeparator = new global::Gtk.VSeparator();
			this.timeLeftSeparator.Name = "timeLeftSeparator";
			this.timeHBox.Add(this.timeLeftSeparator);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.timeHBox[this.timeLeftSeparator]));
			w16.Position = 0;
			w16.Expand = false;
			w16.Fill = false;
			// Container child timeHBox.Gtk.Box+BoxChild
			this.timeVBox = new global::Gtk.VBox();
			this.timeVBox.Name = "timeVBox";
			// Container child timeVBox.Gtk.Box+BoxChild
			this.timeLabel = new global::Gtk.Label();
			this.timeLabel.Name = "timeLabel";
			this.timeVBox.Add(this.timeLabel);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.timeVBox[this.timeLabel]));
			w17.Position = 0;
			w17.Expand = false;
			w17.Fill = false;
			// Container child timeVBox.Gtk.Box+BoxChild
			this.totalTimeLabel = new global::Gtk.Label();
			this.totalTimeLabel.Name = "totalTimeLabel";
			this.timeVBox.Add(this.totalTimeLabel);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.timeVBox[this.totalTimeLabel]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			this.timeHBox.Add(this.timeVBox);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.timeHBox[this.timeVBox]));
			w19.Position = 1;
			w19.Expand = false;
			w19.Fill = false;
			// Container child timeHBox.Gtk.Box+BoxChild
			this.timeRightSeparator = new global::Gtk.VSeparator();
			this.timeRightSeparator.Name = "timeRightSeparator";
			this.timeHBox.Add(this.timeRightSeparator);
			global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.timeHBox[this.timeRightSeparator]));
			w20.Position = 2;
			w20.Expand = false;
			w20.Fill = false;
			this.hbox2.Add(this.timeHBox);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.timeHBox]));
			w21.PackType = ((global::Gtk.PackType)(1));
			w21.Position = 3;
			w21.Expand = false;
			w21.Fill = false;
			this.leftBlockAlignment.Add(this.hbox2);
			this.controlsbox.Add(this.leftBlockAlignment);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.controlsbox[this.leftBlockAlignment]));
			w23.Position = 0;
			w23.Expand = false;
			w23.Fill = false;
			// Container child controlsbox.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox();
			this.hbox4.HeightRequest = 0;
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.centerBlockAlignment = new global::Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
			this.centerBlockAlignment.Name = "centerBlockAlignment";
			// Container child centerBlockAlignment.Gtk.Container+ContainerChild
			this.buttonsbox = new global::Gtk.HBox();
			this.buttonsbox.Name = "buttonsbox";
			// Container child buttonsbox.Gtk.Box+BoxChild
			this.prevbutton = new global::Gtk.Button();
			this.prevbutton.TooltipMarkup = "Previous";
			this.prevbutton.Name = "prevbutton";
			this.prevbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child prevbutton.Gtk.Container+ContainerChild
			this.prevbuttonimage = new global::VAS.UI.Helpers.ImageView();
			this.prevbuttonimage.Name = "prevbuttonimage";
			this.prevbutton.Add(this.prevbuttonimage);
			this.buttonsbox.Add(this.prevbutton);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.buttonsbox[this.prevbutton]));
			w25.Position = 0;
			w25.Expand = false;
			w25.Fill = false;
			// Container child buttonsbox.Gtk.Box+BoxChild
			this.playbutton = new global::Gtk.Button();
			this.playbutton.TooltipMarkup = "Play";
			this.playbutton.Name = "playbutton";
			this.playbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child playbutton.Gtk.Container+ContainerChild
			this.playbuttonimage = new global::VAS.UI.Helpers.ImageView();
			this.playbuttonimage.Name = "playbuttonimage";
			this.playbutton.Add(this.playbuttonimage);
			this.buttonsbox.Add(this.playbutton);
			global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.buttonsbox[this.playbutton]));
			w27.Position = 1;
			w27.Expand = false;
			w27.Fill = false;
			// Container child buttonsbox.Gtk.Box+BoxChild
			this.pausebutton = new global::Gtk.Button();
			this.pausebutton.TooltipMarkup = "Pause";
			this.pausebutton.Name = "pausebutton";
			this.pausebutton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child pausebutton.Gtk.Container+ContainerChild
			this.pausebuttonimage = new global::VAS.UI.Helpers.ImageView();
			this.pausebuttonimage.Name = "pausebuttonimage";
			this.pausebutton.Add(this.pausebuttonimage);
			this.buttonsbox.Add(this.pausebutton);
			global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.buttonsbox[this.pausebutton]));
			w29.Position = 2;
			w29.Expand = false;
			w29.Fill = false;
			// Container child buttonsbox.Gtk.Box+BoxChild
			this.nextbutton = new global::Gtk.Button();
			this.nextbutton.TooltipMarkup = "Next";
			this.nextbutton.Sensitive = false;
			this.nextbutton.Name = "nextbutton";
			this.nextbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child nextbutton.Gtk.Container+ContainerChild
			this.nextbuttonimage = new global::VAS.UI.Helpers.ImageView();
			this.nextbuttonimage.Name = "nextbuttonimage";
			this.nextbutton.Add(this.nextbuttonimage);
			this.buttonsbox.Add(this.nextbutton);
			global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.buttonsbox[this.nextbutton]));
			w31.Position = 3;
			w31.Expand = false;
			w31.Fill = false;
			this.centerBlockAlignment.Add(this.buttonsbox);
			this.hbox4.Add(this.centerBlockAlignment);
			global::Gtk.Box.BoxChild w33 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.centerBlockAlignment]));
			w33.Position = 0;
			w33.Fill = false;
			this.controlsbox.Add(this.hbox4);
			global::Gtk.Box.BoxChild w34 = ((global::Gtk.Box.BoxChild)(this.controlsbox[this.hbox4]));
			w34.Position = 1;
			// Container child controlsbox.Gtk.Box+BoxChild
			this.rightBlockAlignment = new global::Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
			this.rightBlockAlignment.Name = "rightBlockAlignment";
			// Container child rightBlockAlignment.Gtk.Container+ContainerChild
			this.hbox3 = new global::Gtk.HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.jumpsbox = new global::Gtk.HBox();
			this.jumpsbox.Name = "jumpsbox";
			this.jumpsbox.Spacing = 6;
			// Container child jumpsbox.Gtk.Box+BoxChild
			this.jumpsLeftSeparator = new global::Gtk.VSeparator();
			this.jumpsLeftSeparator.Name = "jumpsLeftSeparator";
			this.jumpsbox.Add(this.jumpsLeftSeparator);
			global::Gtk.Box.BoxChild w35 = ((global::Gtk.Box.BoxChild)(this.jumpsbox[this.jumpsLeftSeparator]));
			w35.Position = 0;
			w35.Expand = false;
			w35.Fill = false;
			// Container child jumpsbox.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox();
			this.vbox3.Name = "vbox3";
			// Container child vbox3.Gtk.Box+BoxChild
			this.jumpsButton = new global::Gtk.Button();
			this.jumpsButton.WidthRequest = 21;
			this.jumpsButton.HeightRequest = 21;
			this.jumpsButton.CanFocus = true;
			this.jumpsButton.Name = "jumpsButton";
			this.jumpsButton.FocusOnClick = false;
			this.jumpsButton.Relief = ((global::Gtk.ReliefStyle)(2));
			this.jumpsButton.Xalign = 0F;
			this.jumpsButton.Yalign = 0F;
			// Container child jumpsButton.Gtk.Container+ContainerChild
			this.jumpsButtonImage = new global::VAS.UI.Helpers.ImageView();
			this.jumpsButtonImage.Name = "jumpsButtonImage";
			this.jumpsButtonImage.Xalign = 0F;
			this.jumpsButtonImage.Yalign = 0F;
			this.jumpsButton.Add(this.jumpsButtonImage);
			this.vbox3.Add(this.jumpsButton);
			global::Gtk.Box.BoxChild w37 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.jumpsButton]));
			w37.Position = 0;
			w37.Expand = false;
			w37.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.jumpsLabel = new global::Gtk.Label();
			this.jumpsLabel.Name = "jumpsLabel";
			this.vbox3.Add(this.jumpsLabel);
			global::Gtk.Box.BoxChild w38 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.jumpsLabel]));
			w38.Position = 1;
			w38.Expand = false;
			w38.Fill = false;
			this.jumpsbox.Add(this.vbox3);
			global::Gtk.Box.BoxChild w39 = ((global::Gtk.Box.BoxChild)(this.jumpsbox[this.vbox3]));
			w39.Position = 1;
			w39.Expand = false;
			w39.Fill = false;
			// Container child jumpsbox.Gtk.Box+BoxChild
			this.jumpsRightSeparator = new global::Gtk.VSeparator();
			this.jumpsRightSeparator.Name = "jumpsRightSeparator";
			this.jumpsbox.Add(this.jumpsRightSeparator);
			global::Gtk.Box.BoxChild w40 = ((global::Gtk.Box.BoxChild)(this.jumpsbox[this.jumpsRightSeparator]));
			w40.Position = 2;
			w40.Expand = false;
			w40.Fill = false;
			this.hbox3.Add(this.jumpsbox);
			global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.jumpsbox]));
			w41.Position = 0;
			w41.Expand = false;
			w41.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.zoomBox = new global::Gtk.VBox();
			this.zoomBox.Name = "zoomBox";
			// Container child zoomBox.Gtk.Box+BoxChild
			this.zoomLevelButton = new global::Gtk.Button();
			this.zoomLevelButton.WidthRequest = 21;
			this.zoomLevelButton.HeightRequest = 21;
			this.zoomLevelButton.CanFocus = true;
			this.zoomLevelButton.Name = "zoomLevelButton";
			this.zoomLevelButton.FocusOnClick = false;
			this.zoomLevelButton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child zoomLevelButton.Gtk.Container+ContainerChild
			this.zoomLevelImage = new global::VAS.UI.Helpers.ImageView();
			this.zoomLevelImage.Name = "zoomLevelImage";
			this.zoomLevelButton.Add(this.zoomLevelImage);
			this.zoomBox.Add(this.zoomLevelButton);
			global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.zoomBox[this.zoomLevelButton]));
			w43.Position = 0;
			w43.Expand = false;
			w43.Fill = false;
			// Container child zoomBox.Gtk.Box+BoxChild
			this.zoomLabel = new global::Gtk.Label();
			this.zoomLabel.Name = "zoomLabel";
			this.zoomBox.Add(this.zoomLabel);
			global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.zoomBox[this.zoomLabel]));
			w44.Position = 1;
			w44.Expand = false;
			w44.Fill = false;
			this.hbox3.Add(this.zoomBox);
			global::Gtk.Box.BoxChild w45 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.zoomBox]));
			w45.Position = 1;
			w45.Expand = false;
			w45.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.zoomSeparator = new global::Gtk.VSeparator();
			this.zoomSeparator.Name = "zoomSeparator";
			this.hbox3.Add(this.zoomSeparator);
			global::Gtk.Box.BoxChild w46 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.zoomSeparator]));
			w46.Position = 2;
			w46.Expand = false;
			w46.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.rateBox = new global::Gtk.VBox();
			this.rateBox.Name = "rateBox";
			// Container child rateBox.Gtk.Box+BoxChild
			this.rateLevelButton = new global::Gtk.Button();
			this.rateLevelButton.WidthRequest = 21;
			this.rateLevelButton.HeightRequest = 21;
			this.rateLevelButton.Sensitive = false;
			this.rateLevelButton.CanFocus = true;
			this.rateLevelButton.Name = "rateLevelButton";
			this.rateLevelButton.FocusOnClick = false;
			this.rateLevelButton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child rateLevelButton.Gtk.Container+ContainerChild
			this.rateLevelButtonImage = new global::VAS.UI.Helpers.ImageView();
			this.rateLevelButtonImage.Name = "rateLevelButtonImage";
			this.rateLevelButton.Add(this.rateLevelButtonImage);
			this.rateBox.Add(this.rateLevelButton);
			global::Gtk.Box.BoxChild w48 = ((global::Gtk.Box.BoxChild)(this.rateBox[this.rateLevelButton]));
			w48.Position = 0;
			w48.Expand = false;
			w48.Fill = false;
			// Container child rateBox.Gtk.Box+BoxChild
			this.rateLabel = new global::Gtk.Label();
			this.rateLabel.Name = "rateLabel";
			this.rateBox.Add(this.rateLabel);
			global::Gtk.Box.BoxChild w49 = ((global::Gtk.Box.BoxChild)(this.rateBox[this.rateLabel]));
			w49.Position = 1;
			w49.Expand = false;
			w49.Fill = false;
			this.hbox3.Add(this.rateBox);
			global::Gtk.Box.BoxChild w50 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.rateBox]));
			w50.Position = 3;
			w50.Expand = false;
			w50.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.durationBox = new global::Gtk.VBox();
			this.durationBox.Name = "durationBox";
			// Container child durationBox.Gtk.Box+BoxChild
			this.DurationButton = new global::Gtk.ToggleButton();
			this.DurationButton.WidthRequest = 21;
			this.DurationButton.HeightRequest = 21;
			this.DurationButton.Name = "DurationButton";
			this.DurationButton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child DurationButton.Gtk.Container+ContainerChild
			this.DurationButtonImage = new global::VAS.UI.Helpers.ImageView();
			this.DurationButtonImage.Name = "DurationButtonImage";
			this.DurationButton.Add(this.DurationButtonImage);
			this.durationBox.Add(this.DurationButton);
			global::Gtk.Box.BoxChild w52 = ((global::Gtk.Box.BoxChild)(this.durationBox[this.DurationButton]));
			w52.Position = 0;
			w52.Expand = false;
			w52.Fill = false;
			// Container child durationBox.Gtk.Box+BoxChild
			this.durationLabel = new global::Gtk.Label();
			this.durationLabel.Name = "durationLabel";
			this.durationBox.Add(this.durationLabel);
			global::Gtk.Box.BoxChild w53 = ((global::Gtk.Box.BoxChild)(this.durationBox[this.durationLabel]));
			w53.Position = 1;
			w53.Expand = false;
			w53.Fill = false;
			this.hbox3.Add(this.durationBox);
			global::Gtk.Box.BoxChild w54 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.durationBox]));
			w54.Position = 4;
			w54.Expand = false;
			w54.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.durationSeparator = new global::Gtk.VSeparator();
			this.durationSeparator.Name = "durationSeparator";
			this.hbox3.Add(this.durationSeparator);
			global::Gtk.Box.BoxChild w55 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.durationSeparator]));
			w55.Position = 5;
			w55.Expand = false;
			w55.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.viewportsBox = new global::Gtk.HBox();
			this.viewportsBox.Name = "viewportsBox";
			this.viewportsBox.Spacing = 6;
			// Container child viewportsBox.Gtk.Box+BoxChild
			this.viewportsSwitchButton = new global::Gtk.ToggleButton();
			this.viewportsSwitchButton.Name = "viewportsSwitchButton";
			this.viewportsSwitchButton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child viewportsSwitchButton.Gtk.Container+ContainerChild
			this.viewportsSwitchImage = new global::VAS.UI.Helpers.ImageView();
			this.viewportsSwitchImage.Name = "viewportsSwitchImage";
			this.viewportsSwitchButton.Add(this.viewportsSwitchImage);
			this.viewportsBox.Add(this.viewportsSwitchButton);
			global::Gtk.Box.BoxChild w57 = ((global::Gtk.Box.BoxChild)(this.viewportsBox[this.viewportsSwitchButton]));
			w57.Position = 0;
			w57.Expand = false;
			w57.Fill = false;
			// Container child viewportsBox.Gtk.Box+BoxChild
			this.viewportsSeparator = new global::Gtk.VSeparator();
			this.viewportsSeparator.Name = "viewportsSeparator";
			this.viewportsBox.Add(this.viewportsSeparator);
			global::Gtk.Box.BoxChild w58 = ((global::Gtk.Box.BoxChild)(this.viewportsBox[this.viewportsSeparator]));
			w58.Position = 1;
			w58.Expand = false;
			w58.Fill = false;
			this.hbox3.Add(this.viewportsBox);
			global::Gtk.Box.BoxChild w59 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.viewportsBox]));
			w59.Position = 6;
			w59.Expand = false;
			w59.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.center_playhead_box = new global::Gtk.HBox();
			this.center_playhead_box.Name = "center_playhead_box";
			// Container child center_playhead_box.Gtk.Box+BoxChild
			this.centerplayheadbutton = new global::Gtk.Button();
			this.centerplayheadbutton.TooltipMarkup = "Next";
			this.centerplayheadbutton.Name = "centerplayheadbutton";
			this.centerplayheadbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child centerplayheadbutton.Gtk.Container+ContainerChild
			this.centerplayheadbuttonimage = new global::VAS.UI.Helpers.ImageView();
			this.centerplayheadbuttonimage.Name = "centerplayheadbuttonimage";
			this.centerplayheadbutton.Add(this.centerplayheadbuttonimage);
			this.center_playhead_box.Add(this.centerplayheadbutton);
			global::Gtk.Box.BoxChild w61 = ((global::Gtk.Box.BoxChild)(this.center_playhead_box[this.centerplayheadbutton]));
			w61.Position = 0;
			w61.Expand = false;
			w61.Fill = false;
			// Container child center_playhead_box.Gtk.Box+BoxChild
			this.centerPlayHeadSeparator = new global::Gtk.VSeparator();
			this.centerPlayHeadSeparator.Name = "centerPlayHeadSeparator";
			this.center_playhead_box.Add(this.centerPlayHeadSeparator);
			global::Gtk.Box.BoxChild w62 = ((global::Gtk.Box.BoxChild)(this.center_playhead_box[this.centerPlayHeadSeparator]));
			w62.PackType = ((global::Gtk.PackType)(1));
			w62.Position = 1;
			w62.Expand = false;
			w62.Fill = false;
			this.hbox3.Add(this.center_playhead_box);
			global::Gtk.Box.BoxChild w63 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.center_playhead_box]));
			w63.Position = 7;
			w63.Expand = false;
			w63.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.detachbutton = new global::Gtk.Button();
			this.detachbutton.TooltipMarkup = "Detach window";
			this.detachbutton.Name = "detachbutton";
			this.detachbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child detachbutton.Gtk.Container+ContainerChild
			this.detachbuttonimage = new global::VAS.UI.Helpers.ImageView();
			this.detachbuttonimage.Name = "detachbuttonimage";
			this.detachbutton.Add(this.detachbuttonimage);
			this.hbox3.Add(this.detachbutton);
			global::Gtk.Box.BoxChild w65 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.detachbutton]));
			w65.PackType = ((global::Gtk.PackType)(1));
			w65.Position = 8;
			w65.Expand = false;
			w65.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.volumebutton = new global::Gtk.Button();
			this.volumebutton.TooltipMarkup = "Volume";
			this.volumebutton.Name = "volumebutton";
			this.volumebutton.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child volumebutton.Gtk.Container+ContainerChild
			this.volumebuttonimage = new global::VAS.UI.Helpers.ImageView();
			this.volumebuttonimage.Name = "volumebuttonimage";
			this.volumebutton.Add(this.volumebuttonimage);
			this.hbox3.Add(this.volumebutton);
			global::Gtk.Box.BoxChild w67 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.volumebutton]));
			w67.PackType = ((global::Gtk.PackType)(1));
			w67.Position = 9;
			w67.Expand = false;
			w67.Fill = false;
			this.rightBlockAlignment.Add(this.hbox3);
			this.controlsbox.Add(this.rightBlockAlignment);
			global::Gtk.Box.BoxChild w69 = ((global::Gtk.Box.BoxChild)(this.controlsbox[this.rightBlockAlignment]));
			w69.PackType = ((global::Gtk.PackType)(1));
			w69.Position = 2;
			w69.Expand = false;
			w69.Fill = false;
			this.vbox1.Add(this.controlsbox);
			global::Gtk.Box.BoxChild w70 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.controlsbox]));
			w70.Position = 1;
			w70.Expand = false;
			w70.Fill = false;
			this.alignment1.Add(this.vbox1);
			this.lightbackgroundeventbox.Add(this.alignment1);
			this.totalbox.Add(this.lightbackgroundeventbox);
			global::Gtk.Box.BoxChild w73 = ((global::Gtk.Box.BoxChild)(this.totalbox[this.lightbackgroundeventbox]));
			w73.Position = 1;
			w73.Expand = false;
			this.maineventbox.Add(this.totalbox);
			this.Add(this.maineventbox);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.blackboarddrawingarea.Hide();
			this.closebutton.Hide();
			this.prevbutton.Hide();
			this.pausebutton.Hide();
			this.nextbutton.Hide();
			this.DurationButton.Hide();
			this.durationBox.Hide();
			this.controlsbox.Hide();
			this.Show();
		}
	}
}
