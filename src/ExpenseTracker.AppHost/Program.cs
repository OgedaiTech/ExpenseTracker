using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
  .AddPostgres("postgres")
  .WithPgAdmin(configureContainer: p => p.WithImageTag("9.8"));
var postgresDb = postgres.AddDatabase("ExT");

builder
  .AddProject<ExpenseTracker_WebAPI>("webapi")
  .WithReference(postgresDb);


builder.Build().Run();
