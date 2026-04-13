using Microsoft.Data.Sqlite;
public class DbInitializer
{
    public static void Initialize(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = @"
              CREATE TABLE IF NOT EXISTS sessions (
                id TEXT NOT NULL PRIMARY KEY ,
                type TEXT NOT NULL,
                date TEXT NOT NULL, 
                start TEXT NOT NULL,
                end TEXT NOT NULL
            );";
        command.ExecuteNonQuery();
    }
}