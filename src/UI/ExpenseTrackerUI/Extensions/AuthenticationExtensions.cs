using ExpenseTrackerUI.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace ExpenseTrackerUI.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<TokenRefreshService>();
        services.AddScoped<ActivityTrackingHandler>();

        services.AddScoped<CustomAuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Blazor";
        }).AddScheme<AuthenticationSchemeOptions, BlazorAuthenticationHandler>("Blazor", null);

        services.AddAuthorization();
        services.AddCascadingAuthenticationState();

        return services;
    }
}
