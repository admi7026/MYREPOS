using System;
namespace EventBus.SharedModels
{
    public class EventBusSettings
    {
        public string BrokerName { get; set; } = null!;
        public string SubscriptionClientName { get; set; } = null!;
        public int EventBusRetryCount { get; set; }
        public string EventBusUserName { get; set; } = null!;
        public string EventBusPassword { get; set; } = null!;
        public string EventBusConnection { get; set; } = null!;
    }
}

