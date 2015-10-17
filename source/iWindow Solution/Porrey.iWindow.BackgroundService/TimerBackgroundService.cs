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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using Porrey.iWindow.Event.Models;
using Porrey.iWindow.Interfaces;

namespace Porrey.iWindow.Services
{
	/// <summary>
	/// Provides a base class for all background services.
	/// </summary>
	public abstract class TimerBackgroundService : BackgroundService
	{
		public TimerBackgroundService(TimeSpan interval)
		{
			this.Interval = interval;
		}

		public TimeSpan Interval { get; set; }

		protected async override Task<bool> OnStart()
		{
			bool returnValue = false;

            try
			{
				// ***
				// *** Subscribe to the timer event
				// ***
				var e = this.EventAggregator.GetEvent<Events.TimerEvent>();
				this.AddSubscription(e, e.Subscribe((args) => { }));
			}
			finally
			{
				returnValue = await base.OnStart();
			}

			return returnValue;
        }

		private async Task OnInternalTimerEvent(TimerEventArgs e)
		{
			try
			{
				// ***
				// *** Check if it is time to fire the event
				// ***
				if (e.IsMyInterval(this.Interval))
				{
					// ***
					// *** Fire the event
					// ***
					await this.OnTimer(e);
				}
			}
			catch (Exception ex)
			{
				this.PublishException(ex);
			}
		}

		protected virtual Task OnTimer(TimerEventArgs e)
		{
			return Task.FromResult(0);
		}
	}
}
