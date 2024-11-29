using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models;
using UMBIT.ToDo.BuildingBlocks.Message.Bus.MediatorBus;
using UMBIT.ToDo.Dominio.Application.Queries.Tokens;

namespace UMBIT.ToDo.API.Bootstrapper
{
    public static class SegurancaConfigurate
    {
        public static IServiceCollection AddSeguranca(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection(nameof(AuthenticationSettings))
                                            .Get<AuthenticationSettings>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = authSettings.Issuer,
                        ValidAudiences = authSettings.Audiences,
                        IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                        {
                            var mediator = services.BuildServiceProvider().GetService<IMediatorBus>();

                            Console.Write("pegando secret");
                            try
                            {
                                var secretsResutl = mediator.EnviarQuery<ObterChavesQuery, ICollection<string>>(new ObterChavesQuery(kid)).Result;
                                if (secretsResutl.Result.IsValid)
                                {
                                    return secretsResutl.Dados?.Select(t => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(t)) { KeyId = kid });
                                }

                                return null;
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.ToString());
                                throw ex;
                            }
                        },
                    };
                });

            return services;
        }
    }
}
