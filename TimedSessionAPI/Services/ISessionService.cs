

namespace SessionAPI.Services;

public interface ISessionService
{
    public List<Session>? GetAllRecords(PaginationParams paginationParams);
    public Session? GetRecord(Guid id);
    public Session CreateSession(Session session);
    public string? DeleteSession(Guid id);
    public string? UpdateSession(Guid id, Session session);

}