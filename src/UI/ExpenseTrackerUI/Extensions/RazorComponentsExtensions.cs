namespace ExpenseTrackerUI.Extensions;

public static class RazorComponentsExtensions
{
    public static IServiceCollection AddRazorComponentsConfiguration(this IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        return services;
    }
}
