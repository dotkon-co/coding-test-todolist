using System.Security.Claims;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Interfaces;

namespace UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models
{
    public class Principal
    {
        public string Id { get; private set; }
        public string User { get; private set; }
        public string Email { get; private set; }
        public List<Claim> Claims { get; private set; }

        public Principal(ClaimsPrincipal userPrincipal)
        {

            Id = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            Email = userPrincipal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            User = userPrincipal.FindFirst("name")?.Value ?? string.Empty;
            Claims = userPrincipal.Claims.ToList();
        }

        public bool PossuiPermissaoDeAcesso(IPermissao enumeradorDePermissaoBase)
        {
            return EhAdministrador() ||
                Claims.Any(c =>
                    c.Type == ClaimTypes.Role &&
                    string.Compare(c.Value, enumeradorDePermissaoBase.IdentificadorCompleto, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public bool EhAdministrador()
        {
            return Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Administrador");
        }

        public bool EhValido()
        {
            return
                !string.IsNullOrEmpty(Id) &&
                !string.IsNullOrEmpty(Email);
        }

    }
}
