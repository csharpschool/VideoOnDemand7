var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy(UserPolicy.Admin, policy => policy.RequireRole(UserRole.Admin));
});

builder.Services.AddScoped<ISessionStorage, SessionStorage>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthStateProvider>();
builder.Services.AddScoped<IMembersipService, MembersipService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddHttpClient<ApplicationHttpClient>(client => client.BaseAddress = new Uri("https://localhost:6001/api/"));
builder.Services.AddHttpClient<AuthenticationHttpClient>(client => client.BaseAddress = new Uri("https://localhost:5001"));
builder.Services.AddHttpClient<UserHttpClient>(client => client.BaseAddress = new Uri("https://localhost:5501"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
