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
using System.Threading;
using System.Threading.Tasks;
using Porrey.iWindow.Event.Models;
using Porrey.Uwp.IoT.Devices;
using Windows.Devices.I2c;
using Windows.Foundation.Metadata;

namespace Porrey.iWindow.Services
{
	/// <summary>
	/// Sends out a timer event every 500 ms
	/// </summary>
	public class TimerService : BackgroundService
	{
		private Timer _timer = null;
		private long _eventCounter = 0;

		protected override string OnGetName()
		{
			return "Timer";
		}

		protected override Task<bool> OnStart()
		{
			bool returnValue = false;

			try
			{
				_timer = new Timer(this.TimerCallback, null, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));
			}
			catch (Exception ex)
			{
				this.EventAggregator.GetEvent<Events.DebugEvent>().Publish(new DebugEventArgs(ex));
				returnValue = false;
			}

			return Task<bool>.FromResult(returnValue);
		}

		protected override Task<bool> OnStop()
		{
			bool returnValue = false;

			try
			{
				if (_timer != null)
				{
					_timer.Change(Timeout.Infinite, Timeout.Infinite);
					_timer = null;
				}

				returnValue = true;
			}
			catch (Exception ex)
			{
				this.EventAggregator.GetEvent<Events.DebugEvent>().Publish(new DebugEventArgs(ex));
				returnValue = false;
			}

			return Task<bool>.FromResult(returnValue);
		}

		private async void TimerCallback(object state)
		{
			// ***
			// *** Publish the timer event
			// ***
			this.EventAggregator.GetEvent<Events.TimerEvent>().Publish(new TimerEventArgs(this.IncrementCounter(), await this.CurrentDateTime()));
		}

		private long IncrementCounter()
		{
			if (_eventCounter < long.MaxValue)
			{
				// ***
				// *** Increment the counter
				// ***
				_eventCounter++;
			}
			else
			{
				// ***
				// *** Reset the counter
				// ***
				_eventCounter = 0;
			}

			return _eventCounter;
		}

		private async Task<DateTimeOffset> CurrentDateTime()
		{
			DateTimeOffset currentDateTime = DateTimeOffset.Now;

			try
			{
				// ***
				// *** Make sure I2C is available. This will
				// *** allow the code to be run on a normal PC
				// *** without error.
				// ***
				if (ApiInformation.IsTypePresent(typeof(I2cDevice).FullName))
				{
					// ***
					// *** Get the time
					// ***
					Ds1307 rtc = new Ds1307();
					currentDateTime = await rtc.GetAsync();
				}
			}
			catch
			{
				currentDateTime = DateTimeOffset.Now;
			}

			return currentDateTime;
        }
	}
}
