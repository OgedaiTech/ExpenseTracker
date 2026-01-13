using CsvHelper.Configuration;
using ExpenseTracker.Users.UserEndpoints.BulkCreate;

namespace ExpenseTracker.Users.CsvService;

public sealed class UserCsvRecordMap : ClassMap<UserCsvRecord>
{
  public UserCsvRecordMap()
  {
    Map(m => m.Email).Name("Email", "email", "E-mail", "Email Address");
    Map(m => m.FirstName).Name("FirstName", "First Name", "first_name").Optional();
    Map(m => m.LastName).Name("LastName", "Last Name", "last_name").Optional();
    Map(m => m.NationalIdentityNo).Name("NationalIdentityNo", "National Identity No", "national_identity_no", "NationalID").Optional();
    Map(m => m.TaxIdNo).Name("TaxIdNo", "Tax ID No", "tax_id_no", "TaxID").Optional();
    Map(m => m.EmployeeId).Name("EmployeeId", "Employee ID", "employee_id", "EmployeeNo").Optional();
    Map(m => m.Title).Name("Title", "Job Title", "title", "Position").Optional();
  }
}
