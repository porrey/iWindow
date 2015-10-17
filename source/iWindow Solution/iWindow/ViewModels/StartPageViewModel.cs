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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Porrey.iWindow.Common;
using Porrey.iWindow.Interfaces;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;

namespace Porrey.iWindow.ViewModels
{
	public class StartPageViewModel : ViewModelBase
	{
		private Timer _timer = null;

		protected override string OnGetPageName() => "Initialization";

		private string _message = "Initializing...";
		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				this.SetProperty(ref _message, value);
			}
		}

		public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
		{
			base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

			// ***
			// *** Start the initialization task from
			// *** a timer so the view will show. Allow
			// *** enough time for the screen to show.
			// ***
			_timer = new Timer(this.TimerCallback, null, 2000, Timeout.Infinite);
		}

		private async void TimerCallback(object state)
		{
			await this.Initialize();
		}

		private async Task Initialize()
		{
			try
			{
				// ***
				// *** Get the unity container to add the default device
				// ***
				IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();

				// ***
				// *** Start all defined services
				// ***
				await this.SetMessage("Starting services...");
				var services = ServiceLocator.Current.GetAllInstances<IBackgroundService>();

				foreach (var service in services)
				{
					await this.SetMessage(string.Format("Starting {0} service...", service.Name));
					await service.Start();
				}

				// ***
				// *** Get ready tot show the main page
				// ***
				await this.SetMessage("Starting application...");
			}
			finally
			{
				await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
				{
					if (!this.NavigationService.Navigate(MagicValue.Views.MainPage, null))
					{
						MessageDialog md = new MessageDialog("Could not navigate to page.", "Navigation Failed");
						await md.ShowAsync();
					}
				});
			}
		}

		private async Task SetMessage(string message)
		{
			await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				this.Message = message;
			});
		}
	}
}
