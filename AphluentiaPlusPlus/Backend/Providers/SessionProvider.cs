using Backend.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json;

namespace Backend.Providers
{

    public class SessionProvider
    {
        private static readonly IConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost:6379");
        private static IDatabase db = _redis.GetDatabase();

    
        public static void StoreSessionData(SessionData session)
        {
            db.StringSetAsync(session.SessionId.ToString(), JsonConvert.SerializeObject(session));
        }

        public static SessionData? GetSessionData(Guid sessionId)
        {
            return db.KeyExists(sessionId.ToString()) == null ? new SessionData() { isValidSession = false } :
                JsonConvert.DeserializeObject<SessionData>(db.StringGet(sessionId.ToString()));
        }
        public static bool ValidateSession(SessionData session)
        {
            if(session.SessionId != null && session.WebPlatformId != null)
            {
                session.isValidSession = true;
                return true;
            }
            session.isValidSession = false;
            return false;
        }
    }
}
