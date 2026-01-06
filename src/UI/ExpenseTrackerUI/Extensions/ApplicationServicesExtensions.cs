using ExpenseTrackerUI.Services.Expense;

namespace ExpenseTrackerUI.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ExpenseService>();

        return services;
    }
}
