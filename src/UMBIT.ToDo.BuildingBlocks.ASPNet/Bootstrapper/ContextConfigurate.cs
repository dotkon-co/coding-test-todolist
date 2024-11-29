using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace UMBIT.ToDo.BuildingBlocksc.ASPNet.Bootstrapper
{
    public static class ContextConfigurate
    {
        public static IServiceCollection AddAuthSession(this IServiceCollection services, Func<string, string> obtentorToken)
        {
            services.AddScoped((t) =>
            {
                var context = t.CreateScope().ServiceProvider.GetService<IHttpContextAccessor>();
                return new AuthSessionContext(context, obtentorToken);
            });
            return services;
        }
        public class AuthSessionContext
        {
            public const string AUTH_CONTEXT = "AUTH_CONTEXT";
            private Func<string, string> ObtentorToken;
            public bool EhAutenticado => !string.IsNullOrEmpty(httpContextAccessor.HttpContext?.Session.GetString(AUTH_CONTEXT));
            public IHttpContextAccessor httpContextAccessor { get; set; }

            public AuthSessionContext(IHttpContextAccessor httpContextAccessor, Func<string, string> obtenhaToken)
            {
                ObtentorToken = obtenhaToken;
                this.httpContextAccessor = httpContextAccessor;
            }

            public void SetAuthContext(object tokenResponseDTO)
            {
                httpContextAccessor.HttpContext?.Session.SetString(AUTH_CONTEXT, JsonSerializer.Serialize(tokenResponseDTO));
            }

            public void RemoveAuthContext()
            {
                httpContextAccessor.HttpContext?.Session.Remove(AUTH_CONTEXT);
            }
            public string ObterToken()
            {
                var jPrincipal = httpContextAccessor.HttpContext?.Session.GetString(AUTH_CONTEXT);

                return ObtentorToken(jPrincipal);
            }

            public T? GetAuthContext<T>() where T : class
            {
                var jPrincipal = httpContextAccessor.HttpContext?.Session.GetString(AUTH_CONTEXT);

                if (string.IsNullOrEmpty(jPrincipal)) { return null; }

                return JsonSerializer.Deserialize<T>(jPrincipal);
            }
        }
    }
}
