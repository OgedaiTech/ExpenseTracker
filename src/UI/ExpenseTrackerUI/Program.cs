using ExpenseTrackerUI.Components;
using ExpenseTrackerUI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(30);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ITokenStorageService, TokenStorageService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddTransient<AuthenticatedHttpClientHandler>();

// Configure HttpClient with handler
builder.Services.AddHttpClient("AuthenticatedClient", client =>
{
  client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
  client.Timeout = TimeSpan.FromMinutes(5);
})
.AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

// Default HttpClient (for login/public endpoints)
builder.Services.AddHttpClient(string.Empty, client =>
{
  client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
  client.Timeout = TimeSpan.FromMinutes(5);
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication().AddCookie(options =>
{
  options.LoginPath = "/login";
  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
  options.SlidingExpiration = true;
});
builder.Services.AddAuthorization();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

app.MapStaticAssets();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

await app.RunAsync();
