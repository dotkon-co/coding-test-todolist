using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Events;

namespace UMBIT.ToDo.Dominio.Application.Events.Autenticacao
{
    public class LoginRealizadoEvent : UMBITEvent
    {
        public Guid UsuarioId { get; protected set; }
        public LoginRealizadoEvent(Guid usuarioId)
        {
            UsuarioId = usuarioId;
        }
    }
}
