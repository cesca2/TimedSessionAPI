
public class PaginationParams
{

    private string _initialDate = "0000-01-01";

    public string LastDate
    {
        get => _initialDate;
        set => _initialDate = value;
    }
}