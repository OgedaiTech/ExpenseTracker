namespace ExpenseTracker.Tenants.Endpoints.Create;

internal static class CreateTenantConstants
{
  public const string TenantCodeIsRequired = "TENANT_CODE_IS_REQUIRED";
  public const string CantCreateTenantAdminUser = "CANT_CREATE_TENANT_ADMIN_USER";
  public const string TenantCodeAlreadyExists = "TENANT_CODE_ALREADY_EXISTS";
  public const string TenantAdminUserEmailAlreadyExists = "TENANT_ADMIN_USER_EMAIL_ALREADY_EXISTS";
}
