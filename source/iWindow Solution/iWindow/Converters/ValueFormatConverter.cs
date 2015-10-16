using System;
using Windows.UI.Xaml.Data;

namespace Porrey.iWindow.Converters
{
	public class ValueFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string format = (parameter as string).Replace("[", "{").Replace("]", "}");
            return string.Format(format, value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
