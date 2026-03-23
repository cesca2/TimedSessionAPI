using Microsoft.Data.Sqlite;

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    // IConfiguration available in DI container
    public SqliteConnectionFactory(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DataSource");
    }

    public SqliteConnection CreateConnection()
    {
    return new SqliteConnection(_connectionString);
    }
}