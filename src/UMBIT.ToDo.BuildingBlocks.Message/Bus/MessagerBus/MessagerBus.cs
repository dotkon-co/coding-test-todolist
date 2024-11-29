using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using UMBIT.ToDo.BuildingBlocks.Message.Bus.MessagerBus.Models;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;

namespace UMBIT.ToDo.BuildingBlocks.Message.Bus.MessagerBus
{
    internal class MessagerBus : IMessagerBus
    {
        private const string TOPIC_EXCHANGE = "TOPIC_EXCHANGE";

        private JsonSerializerOptions JsonSerializerOptions;
        private List<string> RPCListeners = new List<string>();
        private bool IsConnected => Connection?.IsOpen ?? false;
        private UMBITMessageBusConnectionFactory UMBITMessageBusConnectionFactory;

        internal readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _activeTaskQueue = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

        internal IModel Channel;
        internal IConnection Connection;
        internal ILogger<MessagerBus> Logger;

        public MessagerBus(UMBITMessageBusConnectionFactory UMBITMessageBusConnectionFactory, ILogger<MessagerBus> logger)
        {
            Logger = logger;
            this.UMBITMessageBusConnectionFactory = UMBITMessageBusConnectionFactory;

            JsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };

            try
            {
                TryConnect();
            }
            catch (Exception ex)
            {
            }
        }

        public bool EhValido()
        {
            return IsConnected;
        }

        public void Publish<T>(T message, string nomeQueue = "null", string exchange = "") where T : UMBITMensagem, new()
        {
            TryConnect();

            nomeQueue = !string.IsNullOrEmpty(nomeQueue) ? nomeQueue : typeof(T).Namespace!;
            exchange = !string.IsNullOrEmpty(exchange) ? exchange : nomeQueue;

            Channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);

            var jsonMessage = JsonSerializer.SerializeToUtf8Bytes(message, JsonSerializerOptions);

            Channel.BasicPublish(
                mandatory: true,
                routingKey: "",
                exchange: exchange,
                basicProperties: null,
                body: jsonMessage);
        }
        public void Subscribe<T>(Action<T> onMessage, string nomeQueue = null, string exchange = "") where T : UMBITMensagem, new()
        {
            TryConnect();

            nomeQueue = !string.IsNullOrEmpty(nomeQueue) ? nomeQueue : typeof(T).Namespace!;
            exchange = !string.IsNullOrEmpty(exchange) ? exchange : nomeQueue;

            Channel.QueueDeclare(nomeQueue, durable: true, false, false, null);

            if (!string.IsNullOrEmpty(exchange))
            {
                Channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);

                Channel.QueueBind(queue: nomeQueue,
                                       exchange: exchange,
                                       routingKey: string.Empty);

            }


            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += async (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                try
                {
                    var data = JsonSerializer.Deserialize<T>(message, JsonSerializerOptions);
                    onMessage(data);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Erro na descearialização de mensagem");
                }

                await Task.CompletedTask;
            };

            Channel.BasicConsume(queue: nomeQueue, true, consumer);
        }
        private bool TryConnect()
        {
            Logger.Log(LogLevel.Information, $"Conectando ao servidor de mensageria '{UMBITMessageBusConnectionFactory.ObtenhaURL()}'.");

            var factory = UMBITMessageBusConnectionFactory.ConnectionFactory;

            if (IsConnected) return true;
            if (factory == null) return false;

            Connection = factory.CreateConnection();
            Channel = Connection.CreateModel();

            Connection.ConnectionShutdown += (connection, evt) =>
            {
                Logger.LogError("Conexão finalizada");
            };

            return EhValido();
        }

    }
}
