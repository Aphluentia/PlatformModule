using Backend.Models;
using Backend.Models.Entities;

namespace Backend.Providers
{
    public interface ISessionProvider
    {
        public void StoreSessionData(SessionData session);
        public SessionData? KeepAlive(SessionData session);
        public SessionData? GetSessionData(Guid sessionId);
        public (bool, Error?) ValidateSession(Guid? sessionId);
    }
}
