using ExpenseTrackerUI.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient(string.Empty, client =>
{
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

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

await app.RunAsync();
