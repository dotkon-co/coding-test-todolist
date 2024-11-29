using Microsoft.AspNetCore.Identity;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Interfaces;
using UMBIT.ToDo.Dominio.Application.Events.Autenticacao;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;

namespace UMBIT.ToDo.Dominio.Application.Events
{
    public class AutenticacaoEventHandler :
        IUMBITEventHandler<LoginRealizadoEvent>,
        IUMBITEventHandler<SenhaAtualizadaEvet>
    {
        private readonly UserManager<Usuario> _userManager;

        public AutenticacaoEventHandler(
            UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(LoginRealizadoEvent notification, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.FindByIdAsync(notification.UsuarioId.ToString());

            if (usuario != null)
            {
                usuario.AtualizarRequisicoesDeAtualizacao();
                await _userManager.UpdateAsync(usuario);
            }
        }

        public async Task Handle(SenhaAtualizadaEvet notification, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.FindByIdAsync(notification.UsuarioId.ToString());

            if (usuario != null)
            {
                usuario.AtualizarRequisicoesDeAtualizacao();
                await _userManager.UpdateAsync(usuario);
            }
        }
    }
}
