var builder = DistributedApplication.CreateBuilder(args);

// Database
var username = builder.AddParameter("username", "postgres", secret: true);
var password = builder.AddParameter("password", "root", secret: true);

var postgres = builder.AddPostgres("postgres", username, password)
    .WithLifetime(ContainerLifetime.Persistent);
var postgresdb = postgres.AddDatabase("productapp");

// Api
var api = builder.AddProject<Projects.ProductApp_Api>("api")
    .WaitFor(postgresdb)
    .WithReference(postgresdb);

// Web
builder.AddProject<Projects.ProductApp_Web>("web")
    .WaitFor(api)
    .WithReference(api);

builder.Build().Run();
