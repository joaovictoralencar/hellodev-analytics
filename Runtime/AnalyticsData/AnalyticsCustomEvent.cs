using System.Collections.Generic;
using Unity.Services.Analytics;

namespace HelloDev.Analytics.Data
{
    public abstract class AnalyticsCustomEvent : CustomEvent
    {
        public string EventName { get; private set; }

        public Dictionary<string, object> InternalData => internalData;
        protected Dictionary<string, object> internalData;
        protected AnalyticsCustomEvent(string name) : base(name)
        {
            EventName = name;
            internalData = new Dictionary<string, object>();
        }
        
        public new void Add(string key, object value)
        {
            base.Add(key, value);
            internalData.Add(key, value);
        }
    }
}
