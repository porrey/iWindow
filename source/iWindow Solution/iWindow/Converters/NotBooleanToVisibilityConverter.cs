using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Porrey.iWindow.Converters
{
	public sealed class NotBooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language) => 
			(value is bool && (bool)value) ? Visibility.Collapsed : Visibility.Visible;

		public object ConvertBack(object value, Type targetType, object parameter, string language) => 
			value is Visibility && (Visibility)value == Visibility.Collapsed;
	}
}