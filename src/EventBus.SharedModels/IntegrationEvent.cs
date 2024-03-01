using System.Text.Json.Serialization;

namespace EventBus.SharedModels;

public record IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTimeOffset.Now;
    }

    [JsonInclude]
    public Guid Id { get; private init; }

    [JsonInclude]
    public DateTimeOffset CreationDate { get; private init; }
}

