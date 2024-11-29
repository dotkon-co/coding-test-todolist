using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using UMBIT.ToDo.BuildingBlocks.SignalR.Interfaces;
using UMBIT.ToDo.BuildingBlocks.SignalR.Modelos;

namespace UMBIT.ToDo.BuildingBlocks.SignalR.Cliente
{
    public class SignalRClient : ISignalRClient
    {
        private HubConnection Conexao;
        private ILogger<SignalRClient> Logger;
        public SignalRClient(IOptions<SignalClientSettings> options, ILogger<SignalRClient> logger)
        {
            Logger = logger;
            var url = $"{options.Value?.SignalURL}/{options.Value?.Hub ?? "hub"}";

            Logger.Log(LogLevel.Information, $"Conectando em servidor hub signalr '{url}'.");

            Conexao = new HubConnectionBuilder()
                .WithUrl(new Uri(url), t =>
                {
                    t.SkipNegotiation = true;
                    t.CloseTimeout = TimeSpan.FromSeconds(30);
                    t.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                })
                .WithAutomaticReconnect()
                .Build();

            Conexao.Closed += async (error) =>
            {
                Logger.LogError(error, "Erro na Conexão!");

                await Task.Delay(new Random().Next(0, 5) * 1000);
                await Conexao.StartAsync();
            };
        }


        public void EmitaAtualizacao(string metodo, object dados, string grupo = null)
        {
            VerifiqueConnectClient();

            try
            {
                Logger.LogInformation($"Emitindo atualização via SignalR. {grupo}, {metodo}");
                Conexao?.InvokeAsync("Atualizar", grupo ?? metodo, metodo, dados).Wait();
            }
            catch (Exception ex)
            {
                Conexao?.StopAsync();

                Logger.LogError(ex, "Erro na emissão de atualização!");
            }
        }

        public void Inicializar()
        {
            Conexao.StartAsync().Wait();
        }

        public void RecebaAtualizacao<T>(string metodo, Action<T> handler, string grupo = null)
        {
            VerifiqueConnectClient();

            try
            {
                Logger.LogInformation("Configurando handler de atualização via SignalR");
                Conexao?.InvokeAsync("Registrar", grupo ?? metodo).Wait();
                Conexao?.On(metodo, handler);
            }
            catch (Exception ex)
            {
                Conexao?.StopAsync();

                Logger.LogError(ex, "Erro na recepção de atualização!");
            }
        }

        private void VerifiqueConnectClient()
        {
            if (Conexao?.State != HubConnectionState.Connected)
            {
                Logger.LogInformation("Cliente Desconectado!");

                Conexao?.StartAsync().Wait();
            }
        }
    }
}
