using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Card = TrelloMS.Entities.Card;

namespace TrelloMS.Services
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IModel _channel;
        private readonly TrelloSender _sender;

        public RabbitMqListener(IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/"
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare("trello", exclusive: false, autoDelete: false);

            _sender = (TrelloSender) serviceProvider.GetService(typeof(TrelloSender));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var card = System.Text.Json.JsonSerializer.Deserialize<Card>(
                    Encoding.UTF8.GetString(ea.Body.ToArray()));
                Debug.WriteLine($"{card.Name} was received");
                _sender.AddNewCard(card).Wait(stoppingToken);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("trello", false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Dispose();
            base.Dispose();
        }
    }
}