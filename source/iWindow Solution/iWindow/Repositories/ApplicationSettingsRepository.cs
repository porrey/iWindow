using System;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Porrey.iWindow.Common;
using Porrey.iWindow.Event.Models;
using Porrey.iWindow.Interfaces;
using Porrey.iWindow.Shared.Models;
using Windows.Storage;

namespace Porrey.iWindow.Repositories
{
	public class ApplicationSettingsRepository : BindableBase, IApplicationSettingsRepository
	{
		[Dependency]
		protected IEventAggregator EventAggregator { get; set; }

        #region Settings
        public string TemperatureUnit
        {
            get
            {
                return this.GetSetting<string>(MagicValue.Property.TemperatureUnit, MagicValue.Defaults.TemperatureUnit);
            }
            set
            {
                this.SaveSetting<string>(MagicValue.Property.TemperatureUnit, value);
            }
        }

		public string[] NtpServers
		{
			get
			{
				return this.GetSetting<string[]>(MagicValue.Property.NtpServers, MagicValue.Defaults.NtpServers);
			}
			set
			{
				this.SaveSetting<string[]>(MagicValue.Property.NtpServers, value);
			}
		}
		#endregion

		public T GetSetting<T>(string name, T defaultValue)
		{
			T returnValue = default(T);

			try
			{
				if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(name))
				{
					// ***
					// *** WintRT will not serialize all objects, so use Newtonsoft.Json
					// ***
					string json = (string)ApplicationData.Current.RoamingSettings.Values[name];
					returnValue = JsonConvert.DeserializeObject<T>(json);
				}
				else
				{
					returnValue = defaultValue;
				}
			}
			catch (Exception ex)
			{
				this.EventAggregator.GetEvent<Events.DebugEvent>().Publish(new DebugEventArgs(ex));
			}

			return returnValue;
		}

		public void SaveSetting<T>(string name, T value)
		{
			try
			{
				// ***
				// *** Not all objects will serialize so use Newtonsoft.Json for everything
				// ***
				string json = JsonConvert.SerializeObject(value);
				ApplicationData.Current.RoamingSettings.Values[name] = json;
				this.OnPropertyChanged(name);
				this.EventAggregator.GetEvent<Events.ApplicationSettingChangedEvent>().Publish(new ApplicationSettingChangedEventArgs(name, value));
			}
			catch (Exception ex)
			{
				this.EventAggregator.GetEvent<Events.DebugEvent>().Publish(new DebugEventArgs(ex));
			}
		}

		public Task ResetToDefaults()
		{
			try
			{
				ApplicationData.Current.RoamingSettings.Values.Clear();
				this.EventAggregator.GetEvent<Events.ApplicationSettingChangedEvent>().Publish(new ApplicationSettingChangedEventArgs(MagicValue.Property.TemperatureUnit, this.TemperatureUnit));
			}
			catch (Exception ex)
			{
				this.EventAggregator.GetEvent<Events.DebugEvent>().Publish(new DebugEventArgs(ex));
			}

			return Task.FromResult(0);
		}
	}
}
