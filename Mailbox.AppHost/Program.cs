var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Mailbox_ApiService>("apiservice");

builder.AddProject<Projects.Mailbox_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
