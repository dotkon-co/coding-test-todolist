using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UMBIT.ToDo.BuildingBlocks.Basicos.Utilitarios;
using UMBIT.ToDo.BuildingBlocksc.WebAPI.Middlewares;

namespace UMBIT.ToDo.API.Bootstrapper
{
    public static class AppConfigurate
    {
        public static IServiceCollection AddApp(this IServiceCollection services, string versao = "v1")
        {
            services.AddCors();

            services.AddControllers(o => o.Filters.Add(new EnableQueryAttribute()))
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.WriteIndented = true;
                        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    })
                    .AddOData((o) =>
                    {
                        o.EnableQueryFeatures(1000);
                    });

            services.AddEndpointsApiExplorer();
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.ToString());
                c.SwaggerDoc(versao, new OpenApiInfo { Title = ProjetoAssemblyHelper.NameProjetoInterface, Version = versao });
            });

            services.AddAuthorization();
            return services;
        }

        public static IApplicationBuilder UseApp(this IApplicationBuilder app, string versao = "v1")
        {
            app.UseCors(t =>
            {
                t.AllowAnyOrigin();
                t.AllowAnyMethod();
                t.AllowAnyHeader();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", $"{ProjetoAssemblyHelper.NameProjetoInterface} {versao}");
            });

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            return app;
        }

    }
}
