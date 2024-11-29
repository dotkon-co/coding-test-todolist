using UMBIT.ToDo.API.Bootstrapper;
using UMBIT.ToDo.Infraestrutura.Contextos;
using UMBIT.ToDo.BuildingBlocks.Fabrica.Bootstrapper;
using UMBIT.ToDo.BuildingBlocks.SignalR.Bootstrapper;
using UMBIT.ToDo.BuildingBlocks.Message.Bootstrapper;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Bootstrapper;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Bootstrapper;
using UMBIT.ToDo.BuildingBlocks.Core.Workers.Bootstrapper;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Bootstrapper;

InicializeConfigurate.Inicialize();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddContextPrincipal()
    .AdicionarNotificacao(builder.Configuration)
    .AddDataBase<BusinessContext>(builder.Configuration)
    .AddIdentityConfiguration(builder.Configuration);

builder.Services
    .AddSignalRClient(builder.Configuration)
    .AddMessages(builder.Configuration)
    .AddSignalRHub()
    .AddWorkers()
    .AddApp()
    .AddDependencias(builder.Configuration);

builder.Services.AddSeguranca(builder.Configuration);

var app = builder.Build();

app.UseApp();
app.UseMigrations();
app.UseIdentityMigrations();

app.UseSignalRHub();
app.UseFabricaGenerica();

app.Run();
