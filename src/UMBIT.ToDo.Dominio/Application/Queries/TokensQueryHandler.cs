using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;
using UMBIT.ToDo.Dominio.Application.Queries.Tokens;
using UMBIT.ToDo.Dominio.Basicos;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;
using UMBIT.ToDo.Dominio.Entidades.Auth.Token;

namespace UMBIT.ToDo.Dominio.Application.Queries
{
    public class TokensQueryHandler : UMBITQueryHandlerBase,
        IUMBITQueryRequestHandler<ObterTokenDeUsuarioQuery, TokenResult>,
        IUMBITQueryRequestHandler<ObterChavesQuery, ICollection<string>>
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IOptions<AuthenticationSettings> _tokenSettings;

        private readonly IRepositorioDeLeitura<ApiToken> RepositorioDeToken;
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1910, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        public TokensQueryHandler(
            IUnidadeDeTrabalhoDeLeitura unidadeDeTrabalho,
            INotificador notificador,
            UserManager<Usuario> userManager,
            IOptions<AuthenticationSettings> tokenSettings) : base(unidadeDeTrabalho, notificador)
        {
            _userManager = userManager;
            _tokenSettings = tokenSettings;

            RepositorioDeToken = unidadeDeTrabalho.ObterRepositorio<ApiToken>();
        }

        public async Task<UMBITMessageResponse<TokenResult>> Handle(ObterTokenDeUsuarioQuery request, CancellationToken cancellationToken)
        {
            var _usuario = await _userManager.FindByEmailAsync(request.Email);

            if (_usuario == null)
            {
                AdicionarErro("Usuário não encontrado.");
                return QueryResponse<TokenResult>();
            }

            var apiToken = await RepositorioDeToken.Query()
                .Where(token => token.IdUsuario == _usuario.Id)
                .OrderByDescending(token => token.DataCriacao)
                .FirstOrDefaultAsync();

            var expiresMins = _tokenSettings.Value.ExpiresMins;
            var issuer = _tokenSettings.Value.Issuer;

            if (apiToken == null)
            {
                AdicionarErro("Usuário não possui um token ativo.");
                return QueryResponse<TokenResult>();
            }

            var identityClaims = await ObtenhaClaims(apiToken.IdUsuario);
            var encodedToken = ObtenhaToken(identityClaims, apiToken.ApiSecret, apiToken.Kid, expiresMins, apiToken.Audience, issuer);
            var encodedRefreshToken = ObtenhaToken(new ClaimsIdentity(), apiToken.ApiSecret, apiToken.Kid, expiresMins * 3, apiToken.Audience, issuer);

            var tokenResult = ObtenhaRespostaToken(apiToken.IdUsuario, encodedToken, encodedRefreshToken, expiresMins, identityClaims);


            return QueryResponse(tokenResult);
        }

        private string ObtenhaToken(ClaimsIdentity identityClaims, string secret, string kid, double expiracaoMins, string audience, string emissor)
        {
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = emissor,
                Audience = audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddMinutes(expiracaoMins),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key) { KeyId = kid }, SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private TokenResult ObtenhaRespostaToken(Guid idUser, string encodedToken, string encodedRefreshToken, double expiracaoMins, ClaimsIdentity claimsIdentity)
        {
            var response = new TokenResult
            {
                EhAdm = claimsIdentity.Claims.Any(t => t.Value == TipoUsuario.ADMINISTRADOR && t.Type == ClaimTypes.Role),
                AccessToken = encodedToken,
                RefreshToken = encodedRefreshToken,
                ExpiresIn = TimeSpan.FromMinutes(expiracaoMins).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = idUser.ToString(),
                    Nome = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Name)?.Value ?? string.Empty,
                    Claims = claimsIdentity.Claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }

        private async Task<ClaimsIdentity> ObtenhaClaims(Guid usuarioId)
        {
            var identityClaims = new ClaimsIdentity();
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());
            if (user == null)
                return new ClaimsIdentity();
            var userClaims = await _userManager.GetClaimsAsync(user);
            identityClaims.AddClaims(userClaims);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Name, user.Nome ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)
            };

            identityClaims.AddClaims(claims);

            return await Task.FromResult(identityClaims);
        }

        public async Task<UMBITMessageResponse<ICollection<string>>> Handle(ObterChavesQuery request, CancellationToken cancellationToken)
        {
            var securityKeys = new List<string>();
            var apiTokens = await RepositorioDeToken.Query().Where(tk => tk.Kid == request.KId).ToListAsync();

            foreach (var t in apiTokens)
                securityKeys.Add(t.ApiSecret);

            return QueryResponse<ICollection<string>>(securityKeys);
        }
    }
}
