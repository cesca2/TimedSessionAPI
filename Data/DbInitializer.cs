using Microsoft.Data.Sqlite;
public class DbInitializer
{
    public static void Initialize(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = @"
              CREATE TABLE IF NOT EXISTS sessions (
                id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                type TEXT NOT NULL,
                date TEXT NOT NULL, 
                duration TEXT NOT NULL,
                start TEXT NOT NULL,
                end TEXT NOT NULL
            );";
        command.ExecuteNonQuery();
    }
}