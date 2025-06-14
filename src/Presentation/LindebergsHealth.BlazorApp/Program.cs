using LindebergsHealth.BlazorApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);

    // Cache-Konfiguration für besseres Logout-Verhalten
    options.ProviderOptions.Cache.CacheLocation = "localStorage";
    options.ProviderOptions.Cache.StoreAuthStateInCookie = false;

    // WICHTIG: Redirect-Modus statt Popup verwenden
    options.ProviderOptions.LoginMode = "redirect";
    options.ProviderOptions.Authentication.NavigateToLoginRequestUrl = false;

    // Standard-Scopes
    options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("profile");

    // Post-Logout Redirect URI
    options.ProviderOptions.Authentication.PostLogoutRedirectUri = builder.HostEnvironment.BaseAddress;

    // Erweiterte Konfiguration für bessere Token-Verwaltung
    options.UserOptions.RoleClaim = "roles";

    // Popup-Probleme vermeiden
    options.ProviderOptions.Authentication.RedirectUri = builder.HostEnvironment.BaseAddress + "authentication/login-callback";
});

await builder.Build().RunAsync();
