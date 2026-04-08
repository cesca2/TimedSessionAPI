

namespace SessionAPI.Services;

public interface ISessionService
{
    public List<Session>? GetAllRecords(PaginationParams paginationParams);
    public string CreateSession(Session session);
    public string? DeleteSession(int id);
    public string? UpdateSession(int id, Session session);

}