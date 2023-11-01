using MetricsApp.Client;
using MetricsApp.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var identityHandler = new IdentityHttpHandler(new HttpClientHandler(), sp.GetRequiredService<IdentityAuthenticationStateProvider>());
    return new HttpClient(identityHandler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
});
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<IdentityAuthenticationStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(s => s.GetRequiredService<IdentityAuthenticationStateProvider>());

await builder.Build().RunAsync();
