

using System.ComponentModel.Design;
using System.Reflection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Roulette.App.Model.Database
{
    public class SessionDatabase : AbstractDatabaseHandler, ISessionDatabase
    {
        private readonly ConfigurationOptions _config;
        private readonly string _activeSessions;
        private readonly int _expireTime;
        private ConnectionMultiplexer? _multiplexer;
        private IDatabase? _database;
        public SessionDatabase(IOptions<SessionDatabaseConfig> config)
        {

            _config = new ConfigurationOptions
            {
                EndPoints = { { config.Value.Host, config.Value.Port } } ,
                User = config.Value.Username,
                Password = config.Value.Password,
                DefaultDatabase = int.Parse(config.Value.DatabaseName)
            };

            _activeSessions = config.Value.SessionDbName;
            _expireTime = config.Value.SessionTimeout;
        }

        public override void CloseConnection()
        {
            try
            {
                _multiplexer?.Dispose();
                IsClosed = true;
                IsConnected = _multiplexer.IsConnected;
            }
            catch (Exception ex)
            {
            }
        }

        public override void CreateConnection()
        {
            try
            {
                _multiplexer = ConnectionMultiplexer.Connect(_config);
                if (_multiplexer == null) { throw new Exception("Cannot connected to session database"); }
                _database = _multiplexer.GetDatabase();
                IsConnected = _multiplexer.IsConnected;
                return;
            }
            catch (Exception ex)
            {
                IsClosed = true;
                IsConnected = false;
                return;
            }
        }

        public async Task<bool?> CreateSession(PlayerSession token)
        {
            try
            {
                var activeTokens = await _database.ListRangeAsync(_activeSessions);
                var tokenKey = $"{token.Username}/{token.Token}";
                var fieldNames = (typeof(PlayerSession)).GetProperties();
                string? userActiveSession = null;
                foreach (var session in activeTokens)
                {
                    if (session.ToString().Contains(token.Username))
                    {
                        userActiveSession = session.ToString();
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(userActiveSession))
                {
                    var isPreviousSessionClosed = await _database.KeyDeleteAsync(userActiveSession);
                    await _database.ListRemoveAsync(_activeSessions, userActiveSession);
                }

                var tokenFields = new HashEntry[fieldNames.Length];
                for (var i = 0; i < fieldNames.Length; i++)
                {
                    if (fieldNames[i] != null)
                    {
                        tokenFields[i] =  new HashEntry(fieldNames[i].Name, fieldNames[i].GetValue(token).ToString());
                    }
                }

                await _database.HashSetAsync(tokenKey, tokenFields);
                await _database.ListRightPushAsync(_activeSessions, tokenKey);
                return _database.KeyExpire(tokenKey, TimeSpan.FromMinutes(_expireTime));

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<bool?> DeleteSession(string tokenValue)
        {
            try
            {
                return await _database.KeyDeleteAsync(tokenValue) ? true : false;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PlayerSession?> GetSessionByTokenValue(string key)
        {
            try
            {
                var token = new PlayerSession();
                var storedTokenAttributes = await _database.HashGetAllAsync(key);
                var tokenAttributes = (typeof(PlayerSession)).GetProperties();
                if (storedTokenAttributes.Count() == 0) { return null; }
                foreach (var stored in storedTokenAttributes)
                { 
                    foreach(var attribute in tokenAttributes)
                    {
                        if (attribute.Name == stored.Name)
                        {
                            attribute.SetValue(token, Convert.ChangeType(stored.Value.ToString(), attribute.PropertyType));
                            break;
                        }
                    }
                }
                return token;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
