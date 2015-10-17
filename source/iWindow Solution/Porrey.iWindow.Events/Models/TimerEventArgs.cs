// Copyright © 2015 Daniel Porrey
//
// This file is part of the iWindow solution.
// 
// iWindow is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// iWindow is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with iWindow. If not, see http://www.gnu.org/licenses/.
//
using System;

namespace Porrey.iWindow.Event.Models
{
	/// <summary>
	/// Defines the structure for a timer event argument.
	/// </summary>
	public class TimerEventArgs : EventArgs
	{
		public TimerEventArgs(long eventCounter, DateTimeOffset currentDateTime)
		{
			this.EventCounter = eventCounter;
			this.CurrentDateTime = currentDateTime;
		}

		/// <summary>
		/// Gets a counter that is incremented for every event. When the value
		/// reaches the maximum value for a long the value is reset to zero.
		/// </summary>
		public long EventCounter { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		public bool IsMyInterval(TimeSpan interval) => this.EventCounter % (int)(interval.TotalMilliseconds / 500d) == 0;

		/// <summary>
		/// 
		/// </summary>
		public DateTimeOffset CurrentDateTime { get; }
	}
}
