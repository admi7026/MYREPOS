using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using EventBus.SharedModels;

namespace EventBus
{
    public class RabbitConsumerService : IHostedService, IDisposable
    {
        private readonly string _brokerName;

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<RabbitConsumerService> _logger;
        private readonly ISubscriptionManager _subscriptionManager;
        private IModel? _consumerChannel;
        private string? _queueName;
        private readonly IServiceProvider _serviceProvider;

        public RabbitConsumerService(IRabbitMQPersistentConnection persistentConnection,
                                     ILogger<RabbitConsumerService> logger,
                                     IOptions<EventBusSettings> options,
                                     ISubscriptionManager subscriptionManager,
                                     IServiceProvider serviceProvider)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subscriptionManager = subscriptionManager ?? throw new ArgumentNullException(nameof(subscriptionManager));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _brokerName = options.Value.BrokerName;
            _queueName = options.Value.SubscriptionClientName;
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _consumerChannel = CreateConsumerChannel();

                // subscribe event
                var events = _subscriptionManager.GetEvents();
                foreach (var eventName in events)
                {
                    DoInternalSubscription(eventName);
                }
                //
                StartBasicConsume();
                await Task.CompletedTask;
            }
            catch(Exception ex)
            {
                _logger.LogCritical("Could not start Rabbit: " + ex.Message);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            await Task.CompletedTask;
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _brokerName,
                                    type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel?.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private void StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }
            try
            {
                // Even on exception we take the message off the queue.
                // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
                // For more information see: https://www.rabbitmq.com/dlx.html
                _consumerChannel?.BasicAck(eventArgs.DeliveryTag, multiple: false);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR BasicAck \"{Message}\"", message);
            }
        }

        private void DoInternalSubscription(string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _consumerChannel!.QueueBind(queue: _queueName,
                                        exchange: _brokerName,
                                        routingKey: eventName);
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetRequiredService(subscription.HandlerType);

                        if (handler == null)
                        {
                            continue;
                        }

                        var eventType = _subscriptionManager.GetEventTypeByName(eventName);

                        if (eventType == null)
                        {
                            continue;
                        }

                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                        if (integrationEvent == null)
                        {
                            continue;
                        }
                            
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        if (concreteType == null)
                        {
                            continue;
                        }

                        await Task.Yield();

                        await (Task)concreteType.GetMethod("Handle")?.Invoke(handler, new object[] { integrationEvent })!;
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
    }
}

