var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Asp_NetCoreIdentity>("asp-netcoreidentity");

builder.Build().Run();
