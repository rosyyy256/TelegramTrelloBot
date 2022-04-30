using System.Text;
using RabbitMQ.Client;
using TGCbot.Entities;

namespace TGCbot.Services
{
    public class RabbitMqPublisher
    {
        private readonly string _hostname = "localhost";
        private readonly string _password = "guest";
        private readonly string _queueName = "trello";
        private readonly string _username = "guest";
        private IConnection _connection;

        public RabbitMqPublisher()
        {
            CreateConnection();
        }

        public void SendMessage(Card card)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(_queueName, exclusive: false, autoDelete: false, arguments: null);

                    var json = System.Text.Json.JsonSerializer.Serialize(card);
                    var body = Encoding.UTF8.GetBytes(json);
                    
                    channel.BasicPublish("", _queueName, body: body);
                }
            }
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password,
                VirtualHost = "/"
            };

            _connection = factory.CreateConnection();
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
                return true;
            
            CreateConnection();

            return _connection != null;
        }
    }
}