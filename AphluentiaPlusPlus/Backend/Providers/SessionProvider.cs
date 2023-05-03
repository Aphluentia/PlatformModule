using Backend.Configs;
using Backend.Models;
using Backend.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace Backend.Providers
{

    public class SessionProvider: ISessionProvider
    {
        private static IConnectionMultiplexer _redis;
        private SessionConfigSection _config;
        private static IDatabase cache;
        public SessionProvider(IOptions<RedisCacheConfigSection> config, IOptions<SessionConfigSection> sessionConfig)
        {
            _config = sessionConfig.Value;
            _redis = ConnectionMultiplexer.Connect(config.Value.ConnectionString);
            cache = _redis.GetDatabase();
        }
         

    
        public void StoreSessionData(SessionData session)
        {
            cache.StringSetAsync(session.SessionId.ToString(), JsonConvert.SerializeObject(session));
        }
        public SessionData? KeepAlive(SessionData session)
        {
            session.ValidityUtcNow = session.ValidityUtcNow.AddMinutes(_config.KeepAliveValidityInMinutes);
            return cache.StringSet(session.SessionId.ToString(), JsonConvert.SerializeObject(session)) ? session : null;
        }

        public SessionData? GetSessionData(Guid sessionId)
        {
            return JsonConvert.DeserializeObject<SessionData>(cache.StringGet(sessionId.ToString()));
        }
        public (bool, Error?) ValidateSession(Guid? sessionId)
        {
            if (sessionId == null)
                return (false, ApplicationError.SessionIdRequired);
            if (!cache.KeyExists(sessionId.ToString()))
                return (false, ApplicationError.SessionIsNotAvailable);
            var sessionData = JsonConvert.DeserializeObject<SessionData>(cache.StringGet(sessionId.ToString()));
            if (sessionData.ValidityUtcNow.CompareTo(DateTime.UtcNow) < 0)
                return (false, ApplicationError.SessionExpired);
            return (true, null);
        }
        
    }
}
