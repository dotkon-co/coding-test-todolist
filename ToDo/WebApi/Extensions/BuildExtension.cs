using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
						x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), 
							sqlOptions => sqlOptions.MigrationsAssembly("Infrastructure")).ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

		});

		}


		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IToDoService, TodoService>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
			builder.Services.AddScoped<IEncryptService, EncryptService>();
		}
	}
}