using Microsoft.Data.Sqlite;
using System.Globalization;
namespace SessionAPI.Services;


public class SessionService : ISessionService
{
    private readonly IDbConnectionFactory _dbContext;

    public SessionService(IDbConnectionFactory dbContext)
    {
        _dbContext = dbContext;

    }

    public Session? GetRecord(Guid id)
    {

        using var connection = _dbContext.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, type, date, start, end FROM sessions 
            WHERE id = $id;
        """;
        command.Parameters.Add(new SqliteParameter("$id", id.ToString()));

        try{
            using var datareader = command.ExecuteReader();

        if (!datareader.HasRows) return null;
        else
        {
            while (datareader.Read())
            {
                {

                    var session = new Session(datareader.GetString(1), DateTime.ParseExact(datareader.GetString(2), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToShortDateString(), datareader.GetString(3), datareader.GetString(4));
                    session.Id = datareader.GetGuid(0);
                    return session;
                }
            }
        }
        return null;
        }
        catch (SqliteException ex)
        {
            var message = "SQLite Error" + ex.Message;
            Console.WriteLine(message);
            throw new ApplicationException("Database operation failed");
        }     


    }
    public List<Session>? GetAllRecords(PaginationParams paginationParams)
    {
        List<Session> rows = new ();
        Console.WriteLine(paginationParams.LastDate);

        using var connection = _dbContext.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, type, date, start, end FROM sessions
            WHERE date(date) >= date($EndDate)
            ORDER BY date(date) DESC, start ASC 
        """;
        command.Parameters.AddWithValue("$EndDate", paginationParams.LastDate);
        try
        {
        using var datareader = command.ExecuteReader();
        var i = 0;

        if (!datareader.HasRows) return rows;
        else
        {
            while (datareader.Read())
            {
                {
                    rows.Add(new Session(datareader.GetString(1), DateTime.ParseExact(datareader.GetString(2), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToShortDateString(), datareader.GetString(3), datareader.GetString(4)));
                    rows[i].Id = datareader.GetGuid(0);
                    i++;
                }
            }
        }

        }
        catch (SqliteException ex)
        {
            var message = "SQLite Error" + ex.Message;
            Console.WriteLine(message);
            throw new ApplicationException("Database operation failed");
        }
        return rows;
    }
        


    
    public Session CreateSession(Session newSession)
    {
       
        string message;

        using var connection = _dbContext.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
        """
                INSERT INTO sessions(id, type, date, start, end) 
                VALUES 
                ( $Id,
                  $Type, 
                  $Date, 
                  $Start, 
                  $End )
                ;
            """;
        
        command.Parameters.AddWithValue("$Id", newSession.Id.ToString());
        command.Parameters.AddWithValue("$Type", newSession.Type);
        command.Parameters.AddWithValue("$Date", DateTime.Parse(newSession.Date).ToString("yyyy-MM-dd"));
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
        catch (SqliteException ex)
        {
            message = "SQLite Error" + ex.Message;
            Console.WriteLine(message);
            throw new ApplicationException("Database operation failed");
        }

        return newSession;

    }
    public string? DeleteSession(Guid id)
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
        command.Parameters.AddWithValue("$ID", id.ToString());

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
        catch (SqliteException ex)
        {
            message = "SQLite Error" + ex.Message;
            throw new ApplicationException("Database operation failed");
        }

        return message;

    }

    public string? UpdateSession(Guid id, Session newSession)
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
        command.Parameters.AddWithValue("$ID", id.ToString());
        command.Parameters.AddWithValue("$Type", newSession.Type);
        command.Parameters.AddWithValue("$Date", DateTime.Parse(newSession.Date).ToString("yyyy-MM-dd"));
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
        catch (SqliteException ex)
        {
            message = "SQLite Error" + ex.Message;
            throw new ApplicationException("Database operation failed");
        }

        return message;

    }
}