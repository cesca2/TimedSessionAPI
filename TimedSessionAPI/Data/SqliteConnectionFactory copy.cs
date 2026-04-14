/*using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

public class SqliteConnectionFactory(IConfiguration config, IOptions<DefaultConnection> options) : IDbConnectionFactory
{
    // IConfiguration available in DI container
    private readonly string _connectionString = config.GetConnectionString(options.Value.Default);

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}
*/