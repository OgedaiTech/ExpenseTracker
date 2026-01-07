using ExpenseTrackerUI.Services.Expense;
using ExpenseTrackerUI.Services.Tenant;

namespace ExpenseTrackerUI.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ExpenseService>();
        services.AddScoped<TenantService>();

        return services;
    }
}
