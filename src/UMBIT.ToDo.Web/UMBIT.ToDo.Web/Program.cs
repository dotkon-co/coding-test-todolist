using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;
using UMBIT.Nexus.Auth.Contrato;
using UMBIT.ToDo.BuildingBlocksc.ASPNet.Bootstrapper;
using UMBIT.ToDo.BuildingBlocksc.ASPNet.Middlewares;
using UMBIT.ToDo.Web.services;

var builder = WebApplication.CreateBuilder(args);
var refitSetting = new RefitSettings()
{
    ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions()
    {
        IncludeFields = true,
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    })
};
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    })
    .AddAuthSession((t) =>
    {
        return JsonSerializer.Deserialize<TokenResponseDTO>(t)?.AccessToken;
    });
builder.Services.AddTransient<ServicoExternoMiddleware>();
builder.Services
    .AddRefitClient<IServicoToDo>(refitSetting)
    .AddHttpMessageHandler<ServicoExternoMiddleware>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("BaseAdressService").Value);
    });
builder.Services
    .AddRefitClient<IServicoAuth>(refitSetting)
    .AddHttpMessageHandler<ServicoExternoMiddleware>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("BaseAdressService").Value);
    });
builder.Services
    .AddRefitClient<IServicoUser>(refitSetting)
    .AddHttpMessageHandler<ServicoExternoMiddleware>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("BaseAdressService").Value);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
