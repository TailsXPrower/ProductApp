# ProductApp

## Prerequisites
- Docker Desktop 4.30+ (required for Aspire resource orchestration and Testcontainers)
- .NET SDK 10.0 preview (matching [`global.json`](global.json))
- PowerShell 7.4+ or Windows Terminal (commands target Windows)
- Node.js is **not** required

## Running the system with Aspire
1. Restore workloads and tools:
   ```powershell
   dotnet workload restore
   dotnet restore
   ```
2. Launch the Aspire orchestrator (this starts PostgreSQL, API, and Blazor front end with service discovery):
   ```powershell
   dotnet run --project aspire/ProductApp.AppHost/ProductApp.AppHost.csproj
   ```
3. Navigate to the Aspire dashboard URL printed in the console (default `http://localhost:17298`). Use the dashboard to open the Blazor front end and API Swagger UI.

## Running tests
Run the integration suite (spins up PostgreSQL via Testcontainers and exercises FastEndpoints over HTTP):
```powershell
dotnet test tests/ProductApp.IntegrationTests/ProductApp.IntegrationTests.csproj
```