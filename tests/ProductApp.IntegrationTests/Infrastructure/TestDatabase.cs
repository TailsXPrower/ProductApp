using Testcontainers.PostgreSql;

namespace ProductApp.IntegrationTests.Infrastructure;

public sealed class TestDatabase : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("productapp_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .WithName($"productapp-test-{Guid.NewGuid():N}")
        .Build();

    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        ConnectionString = _container.GetConnectionString();
    }

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();
}
