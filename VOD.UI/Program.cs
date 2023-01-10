using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;
using System.Security.Claims;
using VOD.Authentication.HttpClients;
using VOD.UI.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy(UserPolicy.NotCustomer, policy =>
        policy.RequireAssertion(context => 
            !context.User.HasClaim(ClaimTypes.Role, UserRole.Customer)
        ));

    options.AddPolicy(UserPolicy.Registered, policy =>
    policy.RequireAssertion(context =>
        (context.User.HasClaim(ClaimTypes.Role, UserRole.Registered) &&
        !context.User.HasClaim(ClaimTypes.Role, UserRole.Customer)) ||
        (context.User.HasClaim(ClaimTypes.Role, UserRole.Registered) &&
         context.User.HasClaim(ClaimTypes.Role, UserRole.Customer))
    ));

    options.AddPolicy(UserPolicy.Customer, policy => policy.RequireRole(UserRole.Customer));
    options.AddPolicy(UserPolicy.Admin, policy => policy.RequireRole(UserRole.Admin));
});
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });
builder.Services.AddHttpClient<AuthenticationHttpClient>(client => client.BaseAddress = new Uri("https://localhost:5001"));
builder.Services.AddHttpClient<UserHttpClient>(client => client.BaseAddress = new Uri("https://localhost:5501"));

await builder.Build().RunAsync();
