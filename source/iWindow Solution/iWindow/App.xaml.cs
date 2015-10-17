using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Porrey.iWindow.Common;
using Porrey.iWindow.Event.Models;
using Porrey.iWindow.Interfaces;
using Porrey.iWindow.Repositories;
using Porrey.iWindow.Services;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Porrey.iWindow
{
    sealed partial class App : MvvmUnityAppBase
	{
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnContainerRegistration(IUnityContainer container)
        {
            // ***
            // *** Add general application objects
            // ***
            container.RegisterInstance<IEventAggregator>(new EventAggregator(), new ContainerControlledLifetimeManager());
            container.RegisterInstance<INavigationService>(this.NavigationService);
            container.RegisterInstance<ISessionStateService>(this.SessionStateService);
            container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            container.RegisterType<IApplicationSettingsRepository, ApplicationSettingsRepository>(new ContainerControlledLifetimeManager());

			// ***
			// *** Background Services
			// ***
			container.RegisterType<IBackgroundService, TimerService>(MagicValue.BackgroundService.Timer, new ContainerControlledLifetimeManager());
			container.RegisterType<IBackgroundService, NtpService>(MagicValue.BackgroundService.Ntp, new ContainerControlledLifetimeManager());
			container.RegisterType<IBackgroundService, KeypadService>(MagicValue.BackgroundService.Keypad, new ContainerControlledLifetimeManager());

			// ***
			// *** View Models
			// ***

		}

		protected override void OnApplicationInitialize(IActivatedEventArgs args)
		{
			base.OnApplicationInitialize(args);
		}

		protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
		{
			// ***
			// *** Navigate to the main page
			// ***
			this.NavigationService.Navigate(MagicValue.Views.StartPage, null);
			Window.Current.Activate();

			return Task.FromResult<object>(null);
		}

		private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<Events.DebugEvent>().Publish(new DebugEventArgs(e.Exception));
            e.Handled = true;
        }
	}
}
