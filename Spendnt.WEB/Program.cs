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
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var appCulture = CultureInfo.CreateSpecificCulture("es-MX");
CultureInfo.DefaultThreadCurrentCulture = appCulture;
CultureInfo.DefaultThreadCurrentUICulture = appCulture;
CultureInfo.CurrentCulture = appCulture;
CultureInfo.CurrentUICulture = appCulture;
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. HttpClient para uso general y por el Repository.
//    JwtAuthenticationService le a�adir� la cabecera de autorizaci�n.
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5230") // URL de tu API en desarrollo
});


// Tus servicios existentes
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddSweetAlert2();

// Configuraci�n de Autenticaci�n
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore(); // Servicios b�sicos de autorizaci�n

// Registrar tu servicio de autenticaci�n personalizado
// JwtAuthenticationService implementa tanto AuthenticationStateProvider como ILoginService
// Ya inyecta HttpClient directamente.
builder.Services.AddScoped<JwtAuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<JwtAuthenticationService>());
builder.Services.AddScoped<ILoginService>(provider =>
    provider.GetRequiredService<JwtAuthenticationService>());

// ELIMINAR o COMENTAR esta l�nea si est�s manejando JWT manualmente con JwtAuthenticationService
// builder.Services.AddApiAuthorization();

// Si NO usas AddApiAuthorization(), y BaseAddressAuthorizationMessageHandler sigue siendo necesario
// (lo cual es raro si JwtAuthenticationService ya pone la cabecera),
// necesitar�as registrar IAccessTokenProvider manualmente.
// PERO, si JwtAuthenticationService ya a�ade la cabecera al HttpClient que se le inyecta,
// y ese mismo HttpClient es usado por IRepository, entonces BaseAddressAuthorizationMessageHandler
// podr�a no ser necesario en la configuraci�n de AddHttpClient.

// Intenta primero sin BaseAddressAuthorizationMessageHandler si JwtAuthenticationService
// ya est� configurando la cabecera del HttpClient compartido.

await builder.Build().RunAsync();