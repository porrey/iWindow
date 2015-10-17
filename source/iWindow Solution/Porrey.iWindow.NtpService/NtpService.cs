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
using System.Threading.Tasks;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using Porrey.iWindow.Event.Models;
using Porrey.iWindow.Interfaces;
using Porrey.Uwp.IoT.Devices;
using Porrey.Uwp.Ntp;
using Windows.Devices.I2c;
using Windows.Foundation.Metadata;

namespace Porrey.iWindow.Services
{
	public class NtpService : TimerBackgroundService
	{
		/// <summary>
		/// The NTP servers will be called once every 24 hous.
		/// </summary>
		public NtpService() : base(TimeSpan.FromHours(24))
		{
		}

		protected override string OnGetName()
		{
			return "NTP";
		}

		protected async override Task<bool> OnStart()
		{
			// ***
			// *** Get the current date and time from the Internet
			// ***
			await this.UpdateClock();

			return true;
		}

		protected async override Task OnTimer(TimerEventArgs e)
		{
			await this.UpdateClock();
		}

		/// <summary>
		/// Get the current date and time from an NTP server(s) and update
		/// the DS1307 RTC (Real Time Clock).
		/// </summary>
		/// <returns></returns>
		private async Task UpdateClock()
		{
			// ***
			// *** Make sure I2C is available. This will
			// *** allow the code to be run on a normal PC
			// *** without error.
			// ***
			if (ApiInformation.IsTypePresent(typeof(I2cDevice).FullName))
			{
				// ***
				// *** Create the clint object and set a timeout value.
				// ***
				NtpClient ntp = new NtpClient();
				ntp.Timeout = TimeSpan.FromSeconds(15);

				// ***
				// *** Make the call to the NTP server(s)
				// ***
				DateTime? dateTimeValue = await ntp.GetAsync(this.ApplicationSettngs.NtpServers);

				// ***
				// *** Set the date and time on the RTC
				// ***
				if (dateTimeValue.HasValue)
				{
					Ds1307 rtc = new Ds1307();
					await rtc.SetAsync(dateTimeValue.Value);
				}
			}
		}
	}
}
