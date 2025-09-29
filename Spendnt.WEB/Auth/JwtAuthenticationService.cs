// Spendnt.WEB/Auth/JwtAuthenticationService.cs
using Microsoft.AspNetCore.Components.Authorization; // Para AuthenticationStateProvider, AuthenticationState, NotifyAuthenticationStateChanged
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt; // Para JwtRegisteredClaimNames (necesita el paquete NuGet System.IdentityModel.Tokens.Jwt)

namespace Spendnt.WEB.Auth
{
    public class JwtAuthenticationService : AuthenticationStateProvider, ILoginService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;
        private const string TOKENKEY = "SPENDNT_TOKEN";

        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public JwtAuthenticationService(IJSRuntime jsRuntime, HttpClient httpClient)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TOKENKEY);
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(_anonymous);
                }
                return BuildAuthenticationState(token);
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }

        public async Task LoginAsync(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TOKENKEY, token);
            var authState = BuildAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TOKENKEY);
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            if (string.IsNullOrWhiteSpace(jwt) || jwt.Split('.').Length < 2)
            {
                return claims;
            }

            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            if (jsonBytes == null) return claims;

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                if (keyValuePairs.TryGetValue(JwtRegisteredClaimNames.Sub, out object? sub) ||
                    keyValuePairs.TryGetValue(ClaimTypes.NameIdentifier, out sub) ||
                    keyValuePairs.TryGetValue("nameid", out sub))
                {
                    if (sub != null) claims.Add(new Claim(ClaimTypes.NameIdentifier, sub.ToString()!));
                }

                if (keyValuePairs.TryGetValue(JwtRegisteredClaimNames.UniqueName, out object? name) ||
                    keyValuePairs.TryGetValue(ClaimTypes.Name, out name))
                {
                    if (name != null) claims.Add(new Claim(ClaimTypes.Name, name.ToString()!));
                }

                if (keyValuePairs.TryGetValue(JwtRegisteredClaimNames.Email, out object? email) ||
                    keyValuePairs.TryGetValue(ClaimTypes.Email, out email))
                {
                    if (email != null) claims.Add(new Claim(ClaimTypes.Email, email.ToString()!));
                }

                if (keyValuePairs.TryGetValue(ClaimTypes.Role, out object? roles) ||
                    keyValuePairs.TryGetValue("role", out roles))
                {
                    if (roles is JsonElement rolesElement && rolesElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var role in rolesElement.EnumerateArray())
                        {
                            if (role.ValueKind == JsonValueKind.String)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role.GetString()!));
                            }
                        }
                    }
                    else if (roles != null && roles is string singleRoleString)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, singleRoleString));
                    }
                }
            }
            return claims;
        }

        private static byte[]? ParseBase64WithoutPadding(string base64)
        {
            try
            {
                switch (base64.Length % 4)
                {
                    case 2: base64 += "=="; break;
                    case 3: base64 += "="; break;
                }
                return Convert.FromBase64String(base64);
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Cadena Base64 malformada en el payload del JWT.");
                return null;
            }
        }
    }
}