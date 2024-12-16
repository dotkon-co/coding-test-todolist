using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Requests.ToDo.Create;
using Domain.Requests.User.Login;
using Domain.Requests.User.Register;
using Domain.Settings;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.Services;
using System.Reflection;
using System.Text;

namespace WebApi.Extensions
{
	public static class BuildExtension
	{

		public static void AddConfiguration(this WebApplicationBuilder builder)
		{
			builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

		}

		public static void AddValidation(this WebApplicationBuilder builder)
		{
			builder.Services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
			builder.Services.AddTransient<IValidator<LoginRequest>, LoginValidator>();
			builder.Services.AddTransient<IValidator<RegisterRequest>, RegisterValidator>();
			builder.Services.AddTransient<IValidator<ToDoCreateRequest>, TodoCreateValidator>();
		}

		public static void AddAuthentication(this WebApplicationBuilder builder)
		{
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = false,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
					ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!))
				};
			});
		}
		public static void AddDocumentation(this WebApplicationBuilder builder)
		{
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Por favor insira 'Bearer' [espaço] e depois seu token JWT",
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme, Id = "Bearer" }
						},
						new string[] { }
					}
				});
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
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IToDoService, TodoService>();
			builder.Services.AddScoped<ITokenService, TokenService>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
			builder.Services.AddScoped<IEncryptService, EncryptService>();
		}
	}
}