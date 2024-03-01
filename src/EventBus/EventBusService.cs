using System.Text.Json;
using EventBus.SharedModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EventBus
{
    public class EventBusService : IEventBusService
    {
        private readonly string _brokerName;
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<EventBusService> _logger;

        public EventBusService(IRabbitMQPersistentConnection persistentConnection,
                               ILogger<EventBusService> logger,
                               IOptions<EventBusSettings> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _brokerName = options.Value.BrokerName;
        }

        public void Publish(IntegrationEvent message)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = message.GetType().Name;

                channel.ExchangeDeclare(exchange: _brokerName, type: "direct");

                var body = JsonSerializer.SerializeToUtf8Bytes(message, message.GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                _logger.LogTrace("Publishing event to RabbitMQ");

                channel.BasicPublish(
                    exchange: _brokerName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            }
        }
    }
}
