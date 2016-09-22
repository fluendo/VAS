﻿using VAS.Core.Interfaces;
using VAS.Core.Store;

namespace VAS.Core.Events
{
	public class LoadVideoEvent : Event
	{
		public MediaFileSet mfs { get; set; }
	}

	public class CloseVideoEvent : Event
	{
		public MediaFileSet mfs { get; set; }
	}

	/// <summary>
	/// Event that indicates that video has been stretched.
	/// </summary>
	public class StretchVideoEvent : Event
	{
		/// <summary>
		/// Gets or sets the MediaFileSet.
		/// </summary>
		/// <value>The mfs.</value>
		public MediaFileSet mfs { get; set; }
	}

	/// <summary>
	/// Event that indicates that video size has been changed.
	/// </summary>
	public class ChangeVideoSizeEvent : Event
	{
		/// <summary>
		/// Gets or sets the time.
		/// </summary>
		/// <value>The time.</value>
		public Time Time { get; set; }

		/// <summary>
		/// Gets or sets the player.
		/// </summary>
		/// <value>The player.</value>
		public IPlayerController player { get; set; }
	}

	public class ChangeVideoMessageEvent : Event
	{
		public string message { get; set; }
	}
}
