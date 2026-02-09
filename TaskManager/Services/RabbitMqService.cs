using RabbitMQ.Client;
using System.Text;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services
{
    public class RabbitMqService : IMensagemService
    {
        private readonly IConfiguration _configuration;
        private readonly string _queueName = "tarefas_queue";

        public RabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void PublicarTarefa(string mensagem)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqSettings:HostName"] ?? "localhost",
                UserName = _configuration["RabbitMqSettings:UserName"] ?? "guest",
                Password = _configuration["RabbitMqSettings:Password"] ?? "guest"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(mensagem);

            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
