using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.AddDataContexts();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
	app.ConfigureDevEnvironment();

app.RunMigration();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
