using Microsoft.AspNetCore.Http;

namespace UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models
{
    public class ContextoPrincipal
    {
        private const string BEARER_SCHEME = "Bearer ";
        private readonly HttpContext HttpContext;

        public string? BearerToken = string.Empty;
        public bool EhAutenticado => HttpContext?.User?.Identity != null ?
            HttpContext.User.Identity.IsAuthenticated : false;

        public ContextoPrincipal(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;
            BearerToken = httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault(t => t!.StartsWith(BEARER_SCHEME))?.Substring(BEARER_SCHEME.Length).Trim();
        }

        public ContextoPrincipal(HttpContext httpContext)
        {
            HttpContext = httpContext;
            BearerToken = httpContext?.Request?.Headers["Authorization"].FirstOrDefault(t => t!.StartsWith(BEARER_SCHEME))?.Substring(BEARER_SCHEME.Length).Trim();

        }

        public Principal? ObtenhaPrincipal()
        {
            if (HttpContext?.User == null)
                return null;

            var _user = HttpContext.User;

            return new Principal(_user);
        }
    }
}
