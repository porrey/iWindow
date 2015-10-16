using System;
using System.Runtime.CompilerServices;

namespace Porrey.iWindow.Event.Models
{
	/// <summary>
	/// The type of the event.
	/// </summary>
	public enum DebugEventType
	{
		/// <summary>
		/// The event is for information only.
		/// </summary>
		Information,
		/// <summary>
		/// The event is a warning.
		/// </summary>
		Warning,
		/// <summary>
		/// The event is an error or exception.
		/// </summary>
		Error
	}

	/// <summary>
	/// Debug event arguments passed to the DebugEvent
	/// </summary>
	public class DebugEventArgs : EventArgs
	{
		/// <summary>
		/// Creates an instance of DebugEventArgs from the given eventType,
		/// title and message.
		/// </summary>
		/// <param name="eventType">The type of event represented in this instance.</param>
		/// <param name="title">The title of this instance.</param>
		/// <param name="description">The details of this instance.</param>
		public DebugEventArgs(DebugEventType eventType, string title, string description)
		{
			this.EventType = eventType;
			this.Title = title;
			this.Description = description;
			this.TimestampUtc = DateTimeOffset.Now.UtcDateTime;
		}

		/// <summary>
		/// Creates an instance of DebugEventArgs from the given eventType,
		/// title and message.
		/// </summary>
		/// <param name="eventType">The type of event represented in this instance.</param>
		/// <param name="title">The title of this instance.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		public DebugEventArgs(DebugEventType eventType, string title, string format, params object[] args)
			: this(eventType, title, string.Format(format, args))
		{
		}

		/// <summary>
		/// Creates an instance of DebugEventArgs from an exception.
		/// </summary>
		/// <param name="ex">The exception used to create the instance.</param>
		/// <param name="callerName">The method or property name of the caller to the method.</param>
		public DebugEventArgs(Exception ex, [CallerMemberName]string callerName = null)
		{
			this.EventType = DebugEventType.Error;
			this.Title = string.Format("Exception in '{0}'", callerName);
			this.Description = ex.Message;
			this.TimestampUtc = DateTimeOffset.Now.UtcDateTime;
		}

		/// <summary>
		/// Gets the type of event represented in this instance.
		/// </summary>
		public DebugEventType EventType { get; }

		/// <summary>
		/// Gets the title of this instance.
		/// </summary>
		public string Title { get; }

		/// <summary>
		/// Gets the details of this instance.
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Gets the UTC date and time this instance was created.
		/// </summary>
		public DateTimeOffset TimestampUtc { get; set; }

		/// <summary>
		/// Gets the Timestamp value in local time.
		/// </summary>
		public DateTimeOffset TimestampLocal => this.TimestampUtc.ToLocalTime();
	}
}