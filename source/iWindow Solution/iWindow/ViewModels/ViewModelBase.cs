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
using System.Collections.Generic;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using Porrey.iWindow.Interfaces;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Porrey.iWindow.ViewModels
{
	public abstract class ViewModelBase : ViewModel
	{
		public ViewModelBase()
		{
			this.PageName = this.OnGetPageName();
		}

		private string _pageName = string.Empty;
		public string PageName
		{
			get
			{
				return _pageName;
			}
			set
			{
				this.SetProperty(ref _pageName, value);
			}
		}

		protected CoreDispatcher Dispatcher { get; set; }

		[Dependency]
		protected IEventAggregator EventAggregator { get; set; }

		[Dependency]
		protected IApplicationSettingsRepository ApplicationSettingsRepository { get; set; }

		[Dependency]
		protected INavigationService NavigationService { get; set; }

		public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
		{
			// ***
			// *** Get the current dispatcher and cache it for the view model
			// ***
			this.Dispatcher = Window.Current.Dispatcher;

			base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
		}

		public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
		{
			// ***
			// *** Call the base OnNavigatedFrom
			// ***
			base.OnNavigatedFrom(viewModelState, suspending);

			// ***
			// *** Release the cached dispatcher instance
			// ***
			this.Dispatcher = null;
		}

		protected virtual string OnGetPageName() => "No Page Name Set";
	}
}