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
using System.ComponentModel;
using System.Linq;
using VAS.Core.Common;
using VAS.Core.Interfaces.Drawing;
using VAS.Core.MVVMC;
using VAS.Core.Store;
using VAS.Core.Store.Drawables;
using VAS.Core.ViewModel;
using VAS.Drawing.CanvasObjects.Timeline;

namespace VAS.Drawing.Widgets
{
	[View ("TimeruleView")]
	public class Timerule : SelectionCanvas, ICanvasView<VideoPlayerVM>
	{
		public event EventHandler CenterPlayheadClicked;

		const int MINIMUM_TIME_SPACING = 80;
		int bigLineHeight = 15;
		int smallLineHeight = 5;
		int fontSize;
		readonly int [] MARKER = new int [] { 1, 2, 5, 10, 30, 60, 120, 300, 600, 1200 };
		NeedleView needle;
		double scroll;
		double secondsPerPixel;
		double timeSpacing = 100.0;
		Time currentTime;
		Time duration;
		VideoPlayerVM viewModel;

		public Timerule (IWidget widget) : base (widget)
		{
			needle = new NeedleView ();
			AddObject (needle);
			SecondsPerPixel = 0.1;
			currentTime = new Time (0);
			AdjustSizeToDuration = false;
			ContinuousSeek = true;
			BackgroundColor = App.Current.Style.PaletteBackgroundDark;
			Accuracy = 5.0f;
			PlayerMode = false;
		}

		public Timerule () : this (null)
		{
		}

		public VideoPlayerVM ViewModel {
			get {
				return viewModel;
			}

			set {
				if (viewModel != null) {
					viewModel.PropertyChanged -= HandlePropertyChangedEventHandler;
				}
				viewModel = value;
				if (viewModel != null) {
					viewModel.PropertyChanged += HandlePropertyChangedEventHandler;
				}
				Duration = viewModel.Duration;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:VAS.Drawing.Widgets.Timerule"/> updates the
		/// current time from the <see cref="VideoPlayerVM"/> instead of <see cref="CurrentTime"/>.
		/// Some timerules need to do it from the property since the updates might be sampled from another component.
		/// </summary>
		/// <value><c>true</c> if updates the current time automatically; otherwise, <c>false</c>.</value>
		public bool AutoUpdate {
			get;
			set;
		}

		public double Scroll {
			set {
				scroll = value;
				needle.ResetDrawArea ();
			}
			protected get {
				return scroll;
			}
		}

		public Time CurrentTime {
			get {
				return currentTime;
			}
			set {
				Area area;
				double start, stop, timeX;

				timeX = Utils.TimeToPos (value, SecondsPerPixel) - Scroll;
				if (needle.X < timeX) {
					start = needle.X;
					stop = timeX;
				} else {
					start = timeX;
					stop = needle.X;
				}
				start -= needle.Width / 2;
				stop += needle.Width / 2;
				area = new Area (new Point (start - 1, needle.TopLeft.Y), stop - start + 2, needle.Height);
				currentTime = value;
				needle.ResetDrawArea ();
				widget?.ReDraw (area);
			}
		}

		/// <summary>
		/// Gets or sets the relative time.
		/// </summary>
		/// <value>The relative time.</value>
		public Time RelativeTime { get; set; }

		public double SecondsPerPixel {
			set {
				secondsPerPixel = value;
				needle.ResetDrawArea ();
			}
			get {
				return secondsPerPixel;
			}
		}

		/// <summary>
		/// Flag to set the mode to AdjustSizeToDuration.
		/// AdjustSizeToDuration mode means that the timerule area will include the whole duration, without scroll.
		/// </summary>
		public bool AdjustSizeToDuration {
			set;
			get;
		}

		/// <summary>
		/// Flag to set the mode to presentation.
		/// Presentation mode means that seeks will be made on StopMove, and not on SelectionMove
		/// </summary>
		/// <value><c>true</c> if presentation mode; otherwise, <c>false</c>.</value>
		public bool ContinuousSeek {
			set;
			get;
		}

		public bool PlayerMode {
			set {
				if (value) {
					RuleHeight = Constants.TIMERULE_RULE_PLAYER_HEIGHT;
					FontSize = StyleConf.TimelineRulePlayerFontSize;
					bigLineHeight = 8;
					smallLineHeight = 3;
				} else {
					RuleHeight = Constants.TIMERULE_HEIGHT;
					FontSize = StyleConf.TimelineRuleFontSize;
					bigLineHeight = 15;
					smallLineHeight = 5;
				}
			}
		}

		Time Duration {
			set {
				if (duration != value) {
					duration = value;
					if (duration != null && duration.MSeconds == 0) {
						currentTime = duration;
					}
					needle.ResetDrawArea ();
					widget?.ReDraw ();
				}
			}
			get {
				return duration;
			}
		}

		int RuleHeight {
			get;
			set;
		}

		int FontSize {
			get {
				return fontSize;
			}
			set {
				fontSize = value;
				int theight;
				int twidth;
				tk.MeasureText ("99:99:99", out twidth, out theight, "", fontSize, FontWeight.Normal);
				TextWidth = twidth;
			}
		}

		int TextWidth {
			get;
			set;
		}

		bool WasPlaying {
			get;
			set;
		}

		public void SetViewModel (object viewModel)
		{
			ViewModel = (VideoPlayerVM)viewModel;
		}

		public override void Draw (IContext context, Area area)
		{
			double start, stop, tpos, height, width;
			double interval = secondsPerPixel * timeSpacing;

			if (Duration == null || CurrentTime == null) {
				return;
			}

			height = widget.Height;
			width = widget.Width;
			float totalSeconds = (float)Duration.MSeconds / 1000;

			if (AdjustSizeToDuration) {
				SecondsPerPixel = totalSeconds / width;
				//Calculate the timeSpacing in pixels
				foreach (int i in MARKER) {
					int pixels = (int)Math.Ceiling (MINIMUM_TIME_SPACING * (totalSeconds / i));
					if (pixels <= width) {
						if (Duration.TotalSeconds > 0) {
							timeSpacing = width / (totalSeconds / i);
							interval = i;
						}
						break;
					}
				}
			}

			Begin (context);
			tk.LineWidth = 0;
			BackgroundColor = App.Current.Style.PaletteBackground;
			DrawBackground ();

			tk.FillColor = App.Current.Style.PaletteBackgroundDark;
			tk.DrawRectangle (new Point (area.Start.X, area.Start.Y + area.Height - RuleHeight), area.Width, RuleHeight);


			tk.StrokeColor = App.Current.Style.PaletteWidgets;
			tk.FillColor = App.Current.Style.PaletteWidgets;
			tk.LineWidth = Constants.TIMELINE_LINE_WIDTH;
			tk.FontSlant = FontSlant.Normal;
			tk.FontSize = FontSize;
			tk.DrawLine (new Point (area.Start.X, height), new Point (area.Start.X + area.Width, height));

			start = (Scroll * SecondsPerPixel);
			start = start - (start % interval);
			stop = ((width + Scroll) * secondsPerPixel);
			double intervalLot = ((interval / secondsPerPixel) / 10);

			//Draw a big line each interval start point
			for (double i = start; i <= stop; i += interval) {
				int pixel = (int)(i / secondsPerPixel);
				double pos = pixel - Scroll;

				tk.DrawLine (new Point (pos, height), new Point (pos, height - bigLineHeight));
				tk.FontAlignment = FontAlignment.Center;
				string timeText = new Time { TotalSeconds = (int)i }.ToSecondsString ();
				tk.DrawText (new Point (pos - TextWidth / 2, 2), TextWidth, height - bigLineHeight - 2, timeText);

				//Draw 9 small lines to separate each interval in 10 partitions
				for (int j = 1; j < 10; j++) {
					double position = pos + intervalLot * j;
					tk.DrawLine (new Point (position, height), new Point (position, height - smallLineHeight));
				}
			}

			/* Draw position triangle */
			needle.TimelineHeight = height;
			needle.MaxPointX = widget.Width;
			if (!Moving) {
				tpos = Utils.TimeToPos (CurrentTime, SecondsPerPixel);
				tpos -= Scroll;
				needle.X = tpos;
			}
			needle.Draw (tk, area);
			End ();
		}

		protected override void StartMove (Selection sel)
		{
			WasPlaying = ViewModel.Playing;
			ViewModel.LoadEvent (null, false);
		}

		protected override void StopMove (bool moved)
		{
			if (moved && !ContinuousSeek) {
				ViewModel.Seek (
					Utils.PosToTime (new Point (needle.X + Scroll, 0), SecondsPerPixel), true);
			}
			if (WasPlaying) {
				ViewModel.Play ();
			}
		}

		protected override void SelectionMoved (Selection sel)
		{
			if (ContinuousSeek) {
				Time clickTime = Utils.PosToTime (new Point (needle.X + Scroll, 0), SecondsPerPixel);
				if (clickTime >= Duration) {
					needle.X = Utils.TimeToPos (Duration, SecondsPerPixel);
					return;
				}
				ViewModel.Seek (Utils.PosToTime (new Point (needle.X + Scroll, 0), SecondsPerPixel),
						   false, throttled: true);
			}
		}

		protected override void HandleLeftButton (Point coords, ButtonModifier modif)
		{
			base.HandleLeftButton (coords, modif);
			Time clickTime = Utils.PosToTime (new Point (coords.X + Scroll, 0), SecondsPerPixel);
			if (clickTime >= Duration) {
				return;
			}
			needle.X = coords.X;
			ViewModel.Seek (clickTime, true);
			needle.ReDraw ();
		}

		protected override void HandleDoubleClick (Point coords, ButtonModifier modif)
		{
			base.HandleDoubleClick (coords, modif);

			if (Selections.Any ()) {
				if (CenterPlayheadClicked != null) {
					CenterPlayheadClicked (this, new EventArgs ());
				}
			}
		}

		void HandlePropertyChangedEventHandler (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof (VideoPlayerVM.Duration)) {
				Duration = ViewModel.Duration;
			} else if (AutoUpdate && e.PropertyName == nameof (VideoPlayerVM.CurrentTime)) {
				CurrentTime = ViewModel.CurrentTime;
			}
		}
	}
}
