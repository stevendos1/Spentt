// Spendnt.WEB/Program.cs
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Spendnt.WEB;
using Spendnt.WEB.Repositories;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Spendnt.WEB.Auth;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication; // Para BaseAddressAuthorizationMessageHandler

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. HttpClient para uso general y por el Repository.
//    JwtAuthenticationService le añadirá la cabecera de autorización.
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7000") // URL de tu API
});


// Tus servicios existentes
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddSweetAlert2();

// Configuración de Autenticación
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore(); // Servicios básicos de autorización

// Registrar tu servicio de autenticación personalizado
// JwtAuthenticationService implementa tanto AuthenticationStateProvider como ILoginService
// Ya inyecta HttpClient directamente.
builder.Services.AddScoped<JwtAuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<JwtAuthenticationService>());
builder.Services.AddScoped<ILoginService>(provider =>
    provider.GetRequiredService<JwtAuthenticationService>());

// ELIMINAR o COMENTAR esta línea si estás manejando JWT manualmente con JwtAuthenticationService
// builder.Services.AddApiAuthorization();

// Si NO usas AddApiAuthorization(), y BaseAddressAuthorizationMessageHandler sigue siendo necesario
// (lo cual es raro si JwtAuthenticationService ya pone la cabecera),
// necesitarías registrar IAccessTokenProvider manualmente.
// PERO, si JwtAuthenticationService ya añade la cabecera al HttpClient que se le inyecta,
// y ese mismo HttpClient es usado por IRepository, entonces BaseAddressAuthorizationMessageHandler
// podría no ser necesario en la configuración de AddHttpClient.

// Intenta primero sin BaseAddressAuthorizationMessageHandler si JwtAuthenticationService
// ya está configurando la cabecera del HttpClient compartido.

await builder.Build().RunAsync();