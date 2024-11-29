using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Events;

namespace UMBIT.ToDo.Dominio.Application.Events.Autenticacao
{
    public class SenhaAtualizadaEvet : UMBITEvent
    {
        public Guid UsuarioId { get; protected set; }
        public SenhaAtualizadaEvet(Guid usuarioId)
        {
            UsuarioId = usuarioId;
        }
    }
}
