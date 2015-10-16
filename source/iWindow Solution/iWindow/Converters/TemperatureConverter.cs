using System;
using Microsoft.Practices.ServiceLocation;
using Porrey.iWindow.Common;
using Porrey.iWindow.Interfaces;
using Windows.UI.Xaml.Data;

namespace Porrey.iWindow.Converters
{
    public sealed class TemperatureConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string returnValue = "--.-";

			// ***
			// *** Get the current display unit from the settings
			// ***
			IApplicationSettingsRepository settings = ServiceLocator.Current.GetInstance<IApplicationSettingsRepository>();

			// ***
			// *** Convert the value to a float
			// ***
			float temperature = System.Convert.ToSingle(value);

			if (!float.IsNaN(temperature))
			{
				// ***
				// *** Convert the temperature to Fahrenheit if the
				// *** current display unit is Fahrenheit (otherwise
				// *** it is already Celsius).
				// ***
				if (settings.TemperatureUnit == MagicValue.TemperatureUnit.Fahrenheit)
				{
					temperature = Temperature.ConvertToFahrenheit(temperature);
				}

				// ***
				// *** Format the output
				// ***
				returnValue = string.Format("{0:0.0°}{1}", temperature, settings.TemperatureUnit);
			}

			return returnValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}