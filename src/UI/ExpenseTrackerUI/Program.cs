using ExpenseTrackerUI.Components;
using ExpenseTrackerUI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ExpenseService>();

builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
      options.Cookie.Name = "auth_token";
      options.LoginPath = "/login";
      options.Cookie.MaxAge = TimeSpan.FromHours(1);
      options.AccessDeniedPath = "/access-denied";
      options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Configure HttpClient with handler
builder.Services.AddHttpClient("AuthenticatedClient", client =>
{
  client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
  client.Timeout = TimeSpan.FromMinutes(5);
});

// Default HttpClient (for login/public endpoints)
builder.Services.AddHttpClient(string.Empty, client =>
{
  client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
  client.Timeout = TimeSpan.FromMinutes(5);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
