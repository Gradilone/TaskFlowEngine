using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using TaskManager.Repositories.Interfaces;
using TaskManager.Models.Enum;

namespace TaskManager.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken tokenParada)
        {
            var fabrica = new ConnectionFactory() { HostName = "rabbitmq" };
            var connection = fabrica.CreateConnection();
            var canal = connection.CreateModel();

            canal.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            canal.QueueDeclare(queue: "tarefas_queue", durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(canal);

            consumer.Received += async (model, ea) =>
            {
                var corpo = ea.Body.ToArray();
                var tarefaId = Encoding.UTF8.GetString(corpo);

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ITarefaRepository>();

                try
                {
                    _logger.LogInformation($"Processar tarefa: {tarefaId}");

                    var tarefa = await repo.ObterPorId(tarefaId);
                    if(tarefa == null) _logger.LogError($"FALHA tarefa nula");
                    if (tarefa != null)
                    {
                        tarefa.Status = StatusTarefa.EmProcessamento;
                        var sucesso = await repo.Atualizar(tarefaId, tarefa);
                        if (sucesso)
                             _logger.LogInformation($"Tarefa {tarefaId}: EmProcessamento ");

                        await Task.Delay(10000, tokenParada);

                        tarefa.Status = StatusTarefa.Concluido;
                        var atualizouFim = await repo.Atualizar(tarefaId, tarefa);
                        if (atualizouFim)
                            _logger.LogInformation($" Tarefa {tarefaId}: Concluido ");

                        canal.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao processar: {ex.Message}");

                    var tarefa = await repo.ObterPorId(tarefaId);

                    if (tarefa != null && tarefa.Tentativas < 3)
                    {
                        tarefa.Tentativas++;
                        await repo.Atualizar(tarefaId, tarefa);
                        canal.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                    }
                    else
                    {
                        if (tarefa != null)
                        {
                            tarefa.Status = StatusTarefa.Erro;
                            await repo.Atualizar(tarefaId, tarefa);
                            _logger.LogCritical($"Tarefa {tarefaId} movida para status ERRO ");
                        }
                        canal.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                }
            };

            canal.BasicConsume(queue: "tarefas_queue", autoAck: false, consumer: consumer);
            _logger.LogInformation("Aguardando mensagens...");

            while (!tokenParada.IsCancellationRequested)
            {
                await Task.Delay(1000, tokenParada);
            }
        }
    }
}
