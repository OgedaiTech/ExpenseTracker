using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var userName = builder.AddParameter("Username", "postgres");
var password = builder.AddParameter("password", "h-HhSCtzy{0qd}kHnwZNYk");

var postgresDbServer = builder
  .AddPostgres(name: "postgres", userName: userName, password: password, port: 53096)
  .WithDataVolume()
  .WithPgAdmin(configureContainer: p => p.WithImageTag("9.8"));
var postgresDb = postgresDbServer.AddDatabase("ExT");

builder
  .AddProject<ExpenseTracker_WebAPI>("webapi")
  .WithReference(postgresDb);


await builder.Build().RunAsync();
