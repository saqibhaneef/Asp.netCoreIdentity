var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("Password");

var server = builder.AddSqlServer("sqlserver", password: password, port: 3000)
    .WithDataVolume("AspnetIdentityDbVolume");

var database = server.AddDatabase("AspnetIdentityDb");

builder.AddProject<Projects.Asp_NetCoreIdentity>("asp-netcoreidentity")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
