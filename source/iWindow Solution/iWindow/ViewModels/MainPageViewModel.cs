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
using Microsoft.Practices.Prism.PubSubEvents;
using Windows.UI.Xaml.Navigation;

namespace Porrey.iWindow.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
		private SubscriptionToken _timerEventSubscriptionToken = null;

		protected override string OnGetPageName() => "iWindow";

		/// <summary>
		/// Gets/sets the current date and time for the view
		/// </summary>
		private DateTimeOffset _currentDateTime = DateTimeOffset.Now;
		public DateTimeOffset CurrentDateTime
		{
			get
			{
				return _currentDateTime;
			}
			set
			{
				this.SetProperty(ref _currentDateTime, value);
			}
		}

		public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
		{
			base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

			// ***
			// *** Subscribe to timer events to keep the current date and time
			// ***
			_timerEventSubscriptionToken = this.EventAggregator.GetEvent<Events.TimerEvent>().Subscribe((args) =>
			{
				this.CurrentDateTime = args.CurrentDateTime;
			}, ThreadOption.UIThread);
		}

		public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
		{
			// ***
			// *** Unsubscribe from the timer event
			// ***
			if (_timerEventSubscriptionToken != null)
			{
				this.EventAggregator.GetEvent<Events.TimerEvent>().Unsubscribe(_timerEventSubscriptionToken);
				_timerEventSubscriptionToken.Dispose();
				_timerEventSubscriptionToken = null;
			}

			base.OnNavigatedFrom(viewModelState, suspending);
		}
	}
}
