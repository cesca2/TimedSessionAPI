using Microsoft.Data.Sqlite;

namespace SessionAPI.Services;
public interface ISessionService
{   
    public List<Session>? GetAllRecords();
    public string CreateSession(Session session);
    public string? DeleteSession(int id);
    public string? UpdateSession(int id, Session session);

}

public class SessionService : ISessionService
{
    private readonly IDbConnectionFactory _dbContext;
    // dependency injection by providing dbcontext in constructor
    public SessionService(IDbConnectionFactory dbContext)
    {
        _dbContext = dbContext;

}

    private Session? GetRecord(int id)
    {

        using var connection = _dbContext.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, type, date, start, end FROM sessions
            WHERE id = $id;
        """;        
        command.Parameters.Add(new SqliteParameter("$id", id));

        using var datareader = command.ExecuteReader();
        
        if (!datareader.HasRows) return null;
        else
        {
            while (datareader.Read()){
                {

                    var session = new Session(datareader.GetString(1), datareader.GetString(2), datareader.GetString(3), datareader.GetString(4));
                    session.Id = datareader.GetInt16(0);
                    return session;
                }
                } 
        }     
        return null;
        
        
    }
    public List<Session>? GetAllRecords()
    {
        var rows = new List<Session>();

        using var connection = _dbContext.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, type, date, start, end FROM sessions
            ORDER BY date ASC, start ASC;
        """;        

        using var datareader = command.ExecuteReader();
        var i=0;
        
        if (!datareader.HasRows) return null;
        else
        {
            while (datareader.Read()){
                {
                    rows.Add(new Session(datareader.GetString(1), datareader.GetString(2), datareader.GetString(3), datareader.GetString(4)));
                    rows[i].Id = datareader.GetInt16(0);
                    i++;
                }
                } 
        }     
        
        return rows;
    }
    public string CreateSession(Session newSession)
    {
        string message;

        using var connection = _dbContext.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =          
        """
                INSERT INTO sessions(type, date, start, end) 
                VALUES 
                ( $Type, 
                  $Date, 
                  $Start, 
                  $End )
                ;
            """;
        command.Parameters.AddWithValue("$Type", newSession.Type);
        command.Parameters.AddWithValue("$Date", newSession.Date);
        command.Parameters.AddWithValue("$Start", newSession.Start);
        command.Parameters.AddWithValue("$End", newSession.End);

        try
        {
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                message = "Successfully inserted row";
            }
            else
            {
                message = "No row update";
            }

        }
        catch(SqliteException ex)
        {
            message = "SQLite Error" + ex.Message;
            throw new ApplicationException("Database operation failed");      
        }
        
        return message;

    }
    public string? DeleteSession(int id)
    {
        string message;

        using var connection = _dbContext.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =          
        """
                DELETE FROM sessions
                WHERE id = $ID
                ;
            """;
        command.Parameters.AddWithValue("$ID", id);
    
        try
        {
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                message = $"Successfully deleted entry with id: {id}";
            }
            else
            {
                return null;
            }

        }
        catch(SqliteException ex)
        {
            message = "SQLite Error" + ex.Message;
            throw new ApplicationException("Database operation failed");      
        }
        
        return message;

    }

    public string? UpdateSession(int id, Session newSession)
    {
        string message;

        using var connection = _dbContext.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =          
        """
        UPDATE sessions 
        SET 
        type = $Type, 
        date = $Date, 
        start = $Start, 
        end = $End 
        WHERE id = $ID
        ;
        """;
        command.Parameters.AddWithValue("$ID", id);
        command.Parameters.AddWithValue("$Type", newSession.Type);
        command.Parameters.AddWithValue("$Date", newSession.Date);
        command.Parameters.AddWithValue("$Start", newSession.Start);
        command.Parameters.AddWithValue("$End", newSession.End);

    
        try
        {
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                message = $"Successfully updated entry with id: {id}";
            }
            else
            {
                return null;
            }

        }
        catch(SqliteException ex)
        {
            message = "SQLite Error" + ex.Message;
            throw new ApplicationException("Database operation failed");      
        }
        
        return message;

    }
}