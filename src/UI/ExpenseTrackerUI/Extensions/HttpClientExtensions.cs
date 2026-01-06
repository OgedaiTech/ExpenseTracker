using ExpenseTrackerUI.Services.Authentication;

namespace ExpenseTrackerUI.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddHttpClientConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure HttpClient with activity tracking handler
        services.AddHttpClient("AuthenticatedClient", client =>
        {
            client.BaseAddress = new Uri(configuration["ApiBaseUrl"]!);
            client.Timeout = TimeSpan.FromMinutes(5);
        })
        .AddHttpMessageHandler<ActivityTrackingHandler>();

        // Default HttpClient (for login/public endpoints)
        services.AddHttpClient(string.Empty, client =>
        {
            client.BaseAddress = new Uri(configuration["ApiBaseUrl"]!);
            client.Timeout = TimeSpan.FromMinutes(5);
        });

        return services;
    }
}
