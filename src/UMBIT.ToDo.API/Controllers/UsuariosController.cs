using Microsoft.AspNetCore.Mvc;
using UMBIT.Nexus.Auth.Contrato;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Basicos.Atributos;
using UMBIT.ToDo.Dominio.Application.Commands.Usuarios;
using UMBIT.ToDo.Dominio.Application.Queries.Usuarios;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;

namespace UMBIT.ToDo.API.Controllers
{
    public class UsuariosController : UsuariosControllerBase
    {
        public UsuariosController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task<IActionResult> AtualizarUsuario(Guid id, [FromBody] AtualizarUsuarioRequestDTO request)
        {
            var command = Mapper.Map<AtualizarUsuarioCommand>(request);
            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        public override async Task<IActionResult> AtualizarDadosUsuario([FromBody] AtualizarDadosDeUsuarioRequestDTO request)
        {
            var command = Mapper.Map<AtualizarDadosDeUsuarioCommand>(request);
            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        [Autorizacao(true)]
        public override async Task<IActionResult> RemoverUsuario(Guid id)
        {
            var resposta = await Mediator.EnviarComando(new RemoverUsuarioCommand(id));
            return UMBITResponse(resposta);
        }

        [Autorizacao(true)]
        public override async Task<ActionResult<ICollection<UsuarioResponseDTO>>> Usuarios()
        {
            var usuarios = await Mediator.EnviarQuery<ObterUsuariosQuery, IQueryable<Usuario>>(new ObterUsuariosQuery());
            return UMBITCollectionResponse<UsuarioResponseDTO, Usuario>(usuarios);
        }
    }
}