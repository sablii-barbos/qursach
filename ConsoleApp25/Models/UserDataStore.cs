namespace AutoriaBot.UserData;

public class UserDataStore
{
    private readonly Dictionary<long, UserSession> _sessions = new();

    public UserSession GetOrCreateSession(long userId)
    {
        if (!_sessions.ContainsKey(userId))
            _sessions[userId] = new UserSession { UserId = userId };
        return _sessions[userId];
    }
}