using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebApi.Middlewares;

namespace WebApi.Extensions
{
	public static class AppExtension
	{
		public static void ConfigureDevEnvironment(this WebApplication app)
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}
		public static void RunMigration(this WebApplication app)
		{
			//dotnet ef migrations add InitialCreate --project Infrastructure --startup-project WebApi
			//dotnet ef database update --project Infrastructure --startup-project WebApi

			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var context = services.GetRequiredService<AppDataContext>(); context.Database.Migrate();
			}
		}

		public static void UseCustomMiddlewares(this WebApplication app)
		{
			app.UseMiddleware(typeof(ErrorHandlingMiddleware));
		}
	}
}