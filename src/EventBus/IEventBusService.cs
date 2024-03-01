using EventBus.SharedModels;

namespace EventBus;

public interface IEventBusService
{
    void Publish(IntegrationEvent integrationEvent);
}

