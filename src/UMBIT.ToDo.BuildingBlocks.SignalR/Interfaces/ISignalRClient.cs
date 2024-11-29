using System;
using System.Text.RegularExpressions;

namespace UMBIT.ToDo.BuildingBlocks.SignalR.Interfaces
{
    public interface ISignalRClient
    {
        void EmitaAtualizacao(string metodo, object dados, string grupo = null);
        void RecebaAtualizacao<T>(string metodo, Action<T> handler, string grupo = null);
        void Inicializar();
    }
}
