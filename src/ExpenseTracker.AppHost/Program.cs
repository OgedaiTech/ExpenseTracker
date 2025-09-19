using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<ExpenseTracker_WebAPI>("webapi");

builder.Build().Run();
