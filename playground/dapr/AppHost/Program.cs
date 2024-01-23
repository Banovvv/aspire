var builder = DistributedApplication.CreateBuilder(args);

var stateStore = builder.AddDaprStateStore("statestore");
var pubSub = builder.AddDaprPubSub("pubsub");

builder.AddProject<Projects.DaprServiceA>("servicea")
       .WithDaprSidecar()
       .WithReference(stateStore)
       .WithReference(pubSub);

builder.AddProject<Projects.DaprServiceB>("serviceb")
       .WithDaprSidecar()
       .WithReference(pubSub);

// This project is only added in playground projects to support development/debugging
// of the dashboard. It is not required in end developer code. Comment out this code
// to test end developer dashboard launch experience. Refer to WorkloadAttributes.cs
// for the path to the dashboard binary (defaults to a relative path to the artifacts
// directory).
builder.AddProject<Projects.Aspire_Dashboard>(KnownResourceNames.AspireDashboard)
    .WithEnvironment("DOTNET_DASHBOARD_GRPC_ENDPOINT_URL", "http://localhost:5555")
    .ExcludeFromManifest();

using var app = builder.Build();

await app.RunAsync();
