public class Session
{
    public Guid Id { get; set; } = Guid.NewGuid(); // set to autoincrement in table
    public string Type { get; set; }
    public string Date { get; set; }
    public string Start { get; set; }
    public string End { get; set; }

    public Session(string type, string date, string start, string end)
    {
        Type = type;
        Start = start;
        End = end;
        Date = date;

    }

}