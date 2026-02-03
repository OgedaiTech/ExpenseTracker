using ExpenseTrackerUI.Services.Approval;
using ExpenseTrackerUI.Services.Expense;
using ExpenseTrackerUI.Services.Receipt;
using ExpenseTrackerUI.Services.Tenant;
using ExpenseTrackerUI.Services.User;

namespace ExpenseTrackerUI.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ApprovalService>();
        services.AddScoped<ExpenseService>();
        services.AddScoped<ReceiptService>();
        services.AddScoped<TenantService>();
        services.AddScoped<UserService>();

        return services;
    }
}
