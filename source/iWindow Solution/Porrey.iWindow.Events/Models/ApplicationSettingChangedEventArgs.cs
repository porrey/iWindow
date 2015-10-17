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