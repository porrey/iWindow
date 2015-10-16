using Microsoft.Practices.Prism.PubSubEvents;
using Porrey.iWindow.Event.Models;
using Porrey.iWindow.Shared.Models;

namespace Porrey.iWindow
{
    public class Events
    {
        public class DebugEvent : PubSubEvent<DebugEventArgs> { }
        public class ApplicationSettingChangedEvent : PubSubEvent<ApplicationSettingChangedEventArgs> { }
    }
}
