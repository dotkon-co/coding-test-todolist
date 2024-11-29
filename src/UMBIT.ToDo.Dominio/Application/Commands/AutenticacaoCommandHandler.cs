using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models;
using UMBIT.ToDo.BuildingBlocks.Message.Bus.MediatorBus;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;
using UMBIT.ToDo.Dominio.Application.Commands.Autenticacao;
using UMBIT.ToDo.Dominio.Application.Events.Autenticacao;
using UMBIT.ToDo.Dominio.Basicos;
using UMBIT.ToDo.Dominio.Configuradores;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;
using UMBIT.ToDo.Dominio.Entidades.Auth.Token;

namespace UMBIT.ToDo.Dominio.Application.Commands
{
    public class AutenticacaoCommandHandler :
        UMBITCommandHandlerBase,
        IUMBITCommandRequestHandler<AdicionarAdministradorCommand>,
        IUMBITCommandRequestHandler<CadastrarUsuarioCommand>,
        IUMBITCommandRequestHandler<SolicitarAlteracaoSenhaCommand>,
        IUMBITCommandRequestHandler<AlterarSenhaCommand>,
        IUMBITCommandRequestHandler<AutenticarUsuarioCommand>,
        IUMBITCommandRequestHandler<RemoverTokensDeUsuarioCommand>,
        IUMBITCommandRequestHandler<AtualizarSessaoDeUsuarioCommand>
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly IOptions<AuthenticationSettings> _tokenSettings;
        private readonly IOptions<IdentitySettings> _identitySettings;

        protected readonly IMediatorBus _mediator;

        private readonly IRepositorio<ApiToken> RepositorioDeToken;

        public AutenticacaoCommandHandler(
            IUnidadeDeTrabalho unidadeDeTrabalho,
            IMediatorBus mediator,
            INotificador notificador,
            SignInManager<Usuario> signInManager,
            UserManager<Usuario> userManager,
            IOptions<AuthenticationSettings> tokenSettings,
            IOptions<IdentitySettings> identitySettings) : base(unidadeDeTrabalho, notificador)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mediator = mediator;
            _tokenSettings = tokenSettings;
            _identitySettings = identitySettings;

            RepositorioDeToken = unidadeDeTrabalho.ObterRepositorio<ApiToken>();
        }

        public async Task<UMBITMessageResponse> Handle(AdicionarAdministradorCommand request, CancellationToken cancellationToken)
        {
            var checarConfiguracao = await _userManager.Users.AnyAsync();

            if (checarConfiguracao)
            {
                AdicionarErro("Não é possível configurar um usuário administrador pois já existe um usuário configurado.");
                return CommandResponse();
            }

            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                UserName = request.Email,
                TwoFactorEnabled = _identitySettings.Value.TwoFactorEnabled,
                EmailConfirmed = !_identitySettings.Value.RequireConfirmedEmail,
            };

            var result = await _userManager.CreateAsync(usuario, request.Senha);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, TipoUsuario.ADMINISTRADOR));
            }
            else
            {
                AdicionarErro("Falha ao adicionar usuário.");
            }

            return CommandResponse();
        }

        public async Task<UMBITMessageResponse> Handle(CadastrarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                UserName = request.Email,
                TwoFactorEnabled = _identitySettings.Value.TwoFactorEnabled,
                EmailConfirmed = !_identitySettings.Value.RequireConfirmedEmail,
            };

            var result = await _userManager.CreateAsync(usuario, request.Senha);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    AdicionarErro(item.Code, item.Description);
                }
            }

            return CommandResponse();
        }

        public async Task<UMBITMessageResponse> Handle(SolicitarAlteracaoSenhaCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email);

            if (usuario == null)
            {
                AdicionarErro("Usuaário inválido");
                return CommandResponse();
            }

            if (usuario.AtualizacaoSenhaRequisitada)
            {
                AdicionarErro("Atualização já requisita, favor aguardar a aprovação do gestor");
                return CommandResponse();
            }

            usuario.SolicitarAtualizacaoDeSenha();
            var result = await _userManager.UpdateAsync(usuario);

            if (!result.Succeeded)
                AdicionarErro("Falha durante a atualização do usuário");

            return CommandResponse();
        }

        public async Task<UMBITMessageResponse> Handle(AlterarSenhaCommand request, CancellationToken cancellationToken)
        {
            var _usuario = await _userManager.FindByEmailAsync(request.Email);

            if (_usuario == null)
            {
                AdicionarErro("Usuário inválido");
                return CommandResponse();
            }

            if (!_usuario.AtualizacaoSenhaAprovada)
            {
                AdicionarErro("O processo de atualização de senha deve ser previamente aprovado por um gestor.");
                return CommandResponse();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(_usuario);

            _usuario.AtualizarRequisicoesDeAtualizacao();

            await _userManager.ResetPasswordAsync(_usuario, token, request.Senha);
            await _userManager.UpdateAsync(_usuario);

            return CommandResponse();
        }

        public async Task<UMBITMessageResponse> Handle(AutenticarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var _usuario = await _userManager.FindByEmailAsync(request.Email);

            if (_usuario == null)
            {
                AdicionarErro("Usuário e/ou senha inválidos");
                return CommandResponse();
            }

            if (!_usuario.EmailConfirmed)
            {
                AdicionarErro("Aguardando liberação do usuário, favor entrar em contato com um administrador.");
                return CommandResponse();
            }

            var result = await _signInManager.PasswordSignInAsync(
                _usuario,
                request.Senha,
                isPersistent: false,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                await GereToken(_usuario.Id, request.Audience);
                _mediator.PublicarEvento(new LoginRealizadoEvent(_usuario.Id));
            }
            else
            {
                switch (result.ToString())
                {
                    case "LockedOut":
                        AdicionarErro("Usuário bloqueado temporariamente por tentantivas inválidas");
                        break;
                    default:
                        AdicionarErro("Usuário e/ou senha inválidos");
                        break;
                }
            }

            return CommandResponse();
        }

        public async Task<UMBITMessageResponse> Handle(RemoverTokensDeUsuarioCommand request, CancellationToken cancellationToken)
        {
            var _usuario = await _userManager.FindByEmailAsync(request.Email);

            if (_usuario == null)
            {
                AdicionarErro("Usuário não encontrado");
                return CommandResponse();
            }

            var tokensDeUsuario = await RepositorioDeToken.Query().Where(tk => tk.IdUsuario == _usuario.Id).ToListAsync();
            foreach (var token in tokensDeUsuario)
            {
                RepositorioDeToken.Remover(token);
            }

            await UnidadeDeTrabalho.SalveAlteracoes();

            return CommandResponse();
        }

        public async Task<UMBITMessageResponse> Handle(AtualizarSessaoDeUsuarioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var securityToken = tokenHandler.ReadJwtToken(request.AccessToken);
                var kid = securityToken.Header.Kid;

                var savedToken = await RepositorioDeToken.Query().Where(tk => tk.Kid == kid).FirstOrDefaultAsync();

                if (savedToken == null)
                {
                    AdicionarErro("Token inválido");
                    return CommandResponse();
                }

                var secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(savedToken.ApiSecret));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secret,
                    ValidateLifetime = true
                };

                tokenHandler.ValidateToken(request.RefreshToken, tokenValidationParameters, out SecurityToken validatedToken);

                await GereToken(savedToken.IdUsuario, savedToken.Audience);
            }
            catch
            {
                AdicionarErro("Falha ao validar token");
            }

            return CommandResponse();
        }

        private async Task GereToken(Guid usuarioId, string audience)
        {
            try
            {
                await DeleteTokensDeUsuario(usuarioId);

                var issuer = _tokenSettings.Value.Issuer;
                var expiresMins = _tokenSettings.Value.ExpiresMins;

                var token = new ApiToken
                {
                    Kid = GereKid(),
                    Audience = audience,
                    IdUsuario = usuarioId,
                    ApiSecret = GereApiSecret()
                };

                await RepositorioDeToken.Adicionar(token);
                await UnidadeDeTrabalho.SalveAlteracoes();
            }
            catch
            {
                AdicionarErro("Falha ao gerar token");
            }
        }

        public async Task DeleteTokensDeUsuario(Guid usuarioId)
        {
            var tokensUsuario = await RepositorioDeToken.Query().Where(tk => tk.IdUsuario == usuarioId).ToListAsync();
            foreach (var item in tokensUsuario)
            {
                RepositorioDeToken.Remover(item);
                await UnidadeDeTrabalho.SalveAlteracoes();
            }
        }

        private string GereKid()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private string GereApiSecret()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }
    }
}
