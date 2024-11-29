using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using UMBIT.Nexus.Auth.Contrato;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models;
using UMBIT.ToDo.Dominio.Application.Commands.Autenticacao;
using UMBIT.ToDo.Dominio.Application.Queries.Configuracao;
using UMBIT.ToDo.Dominio.Application.Queries.Tokens;
using UMBIT.ToDo.Dominio.Application.Queries.Usuarios;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;
using UMBIT.ToDo.Dominio.Entidades.Auth.Configuracao;
using UMBIT.ToDo.Dominio.Entidades.Auth.Token;

namespace UMBIT.ToDo.API.Controllers
{
    [AllowAnonymous]
    public class AutenticacaoController : AutenticacaoControllerBase
    {
        private readonly ContextoPrincipal _contextoPrincipal;

        public AutenticacaoController(
            IServiceProvider serviceProvider,
            ContextoPrincipal contextoPrincipal) : base(serviceProvider)
        {
            _contextoPrincipal = contextoPrincipal;
        }

        public override async Task<IActionResult> AdicionarAdministrador([FromBody] AdicionarAdministradorRequestDTO request)
        {
            var command = Mapper.Map<AdicionarAdministradorCommand>(request);
            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        public override async Task<IActionResult> AdicionarUsuario([FromBody] AdicionarUsuarioRequestDTO registerRequest)
        {
            var command = Mapper.Map<CadastrarUsuarioCommand>(registerRequest);
            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        public override async Task<ActionResult<AuthStatusResponseDTO>> AuthStatus()
        {
            var response = await Mediator.EnviarQuery<ObterStatusDeConfiguracaoQuery, StatusDeConfiguracao>(new ObterStatusDeConfiguracaoQuery());
            return UMBITResponse<AuthStatusResponseDTO, StatusDeConfiguracao>(response);
        }

        public override async Task<ActionResult<UserStatusResponseDTO>> UserStatus([FromBody] UserStatusRequestDTO userStatusRequest)
        {
            var response = await Mediator.EnviarQuery<ObterUsuarioPorEmailQuery, Usuario>(new ObterUsuarioPorEmailQuery(userStatusRequest.Email));
            return UMBITResponse<UserStatusResponseDTO, Usuario>(response);
        }

        public override async Task<ActionResult<TokenResponseDTO>> Login([FromBody] LoginRequestDTO login)
        {
            var command = Mapper.Map<AutenticarUsuarioCommand>(login);
            var response = await Mediator.EnviarComando(command);

            if (!response.Result.IsValid)
                return UMBITResponse<TokenResponseDTO>();

            var tokenResult = await Mediator.EnviarQuery<ObterTokenDeUsuarioQuery, TokenResult>(new ObterTokenDeUsuarioQuery(login.User));
            return UMBITResponse<TokenResponseDTO, TokenResult>(tokenResult);
        }
        public override async Task<IActionResult> Logout()
        {
            var principal = _contextoPrincipal.ObtenhaPrincipal();
            if (principal == null || !principal.EhValido())
                return UMBITResponse();

            var response = await Mediator.EnviarComando(new RemoverTokensDeUsuarioCommand(principal.Email));
            return UMBITResponse(response);
        }

        private string ObtenhaEmail(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.ReadJwtToken(token);

            return securityToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty;
        }
    }
}