using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace UMBIT.ToDo.BuildingBlocks.Message.Bus.MessagerBus.Models
{
    public class UMBITMessageBusConnectionFactory
    {
        public ConnectionFactory ConnectionFactory { get; private set; }
        public UMBITMessageBusConnectionFactory(IOptions<UMBITMessageBusConfig> messageBusConfig)
        {
            if (ConnectionFactory == null)
            {
                ConnectionFactory = new ConnectionFactory()
                {
                    HostName = messageBusConfig?.Value?.Host ?? "localhost",
                    Password = messageBusConfig?.Value?.Senha ?? "guest",
                    UserName = messageBusConfig?.Value?.Usuario ?? "guest",
                    Port = messageBusConfig?.Value?.Port ?? 5672
                };

                ConnectionFactory.AutomaticRecoveryEnabled = true;
                ConnectionFactory.RequestedHeartbeat = TimeSpan.FromSeconds(60);
                ConnectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            }
        }

        public string ObtenhaURL() => $"amqp://{ConnectionFactory.HostName}:{ConnectionFactory.Port}" ?? string.Empty;
    }
}
