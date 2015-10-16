using System;

namespace Porrey.iWindow.Shared.Models
{
	public class ApplicationSettingChangedEventArgs : EventArgs
    {
		public ApplicationSettingChangedEventArgs(string propertyName, object newValue)
		{
			this.PropertyName = propertyName;
			this.NewValue = newValue;
		}

		public string PropertyName { get; set; }
		public object NewValue { get; set; }
	}
}