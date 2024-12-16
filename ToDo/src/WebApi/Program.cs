using FluentValidation.AspNetCore;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssembly(typeof(Program).Assembly));

builder.AddConfiguration();
builder.AddAuthentication();
builder.AddDataContexts();
builder.AddDocumentation();
builder.AddServices();
builder.AddValidation();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
app.ConfigureDevEnvironment();

app.RunMigration();

app.UseCustomMiddlewares();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
