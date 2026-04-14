using Microsoft.Data.Sqlite;
public class DbInitializer
{
    public static void Initialize(SqliteConnection connection, bool reInitialize = false)
    {
        if (reInitialize)
        {
            var reinitCommand = connection.CreateCommand();
            reinitCommand.CommandText = @"
              DROP TABLE IF EXISTS sessions 
            ;";
            reinitCommand.ExecuteNonQuery();
            
        }
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