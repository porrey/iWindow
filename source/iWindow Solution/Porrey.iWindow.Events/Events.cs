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
using Microsoft.Practices.Prism.PubSubEvents;
using Porrey.iWindow.Event.Models;
using Porrey.iWindow.Shared.Models;

namespace Porrey.iWindow
{
    public class Events
    {
        /// <summary>
        /// This event can be used to push any type of internal message for debugging purposes.
        /// </summary>
        public class DebugEvent : PubSubEvent<DebugEventArgs> { }

        /// <summary>
        /// This event is fired when any application settings value has changed.
        /// </summary>
        public class ApplicationSettingChangedEvent : PubSubEvent<ApplicationSettingChangedEventArgs> { }

        /// <summary>
        /// This event is fired periodically by a timer service and can be monitored
        /// if something needs to be periodically updated or refreshed.
        /// </summary>
        public class TimerEvent : PubSubEvent<TimerEventArgs> { }
    }
}
