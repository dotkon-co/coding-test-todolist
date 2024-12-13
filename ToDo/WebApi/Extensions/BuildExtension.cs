using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Service.Services;

namespace WebApi.Extensions
{
	public static class BuildExtension
	{
		
		public static void AddDocumentation(this WebApplicationBuilder builder)
		{
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(x =>
			{
				x.CustomSchemaIds(n => n.FullName);
			});

		}

		public static void AddDataContexts(this WebApplicationBuilder builder)
		{
			builder
				.Services
				.AddDbContext<AppDataContext>(
					x =>
					{
						x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
					});

		}


		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IEncryptService, EncryptService>();
		}
	}
}