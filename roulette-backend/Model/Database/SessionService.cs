
using Microsoft.EntityFrameworkCore;

namespace Roulette.App.Model.Database
{
    public class SessionService : ISessionDatabase
    {
        private readonly RouletteDbContext _dbContext;

        public SessionService(RouletteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CloseConnection()
        {
            throw new NotImplementedException();
        }

        public void CreateConnection()
        {
            throw new NotImplementedException();
        }

        public async Task<bool?> CreateSession(PlayerSession token)
        {
            try
            {
                var existingToken = await _dbContext.ActiveSessions
                    .FirstOrDefaultAsync(session => session.Username == token.Username);

                if (existingToken != null)
                {
                    _dbContext.ActiveSessions.Remove(existingToken);
                }
                // Add the new token to the database
                await _dbContext.ActiveSessions.AddAsync(token);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool?> DeleteSession(string tokenValue)
        {
            try
            {
                var token = _dbContext.ActiveSessions.FirstOrDefault(session => session.Token == tokenValue);
                if (token == null) { return false; }
                _dbContext.ActiveSessions.Remove(token);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<PlayerSession?> GetSessionByTokenValue(string key)
        {
            return await _dbContext.ActiveSessions.FirstOrDefaultAsync(_dbContext => _dbContext.Id == key);
        }

        public PlayerSession? GenerateSessionToken(string username)
        {
            try
            {
                var tokenValue = Guid.NewGuid().ToString();
                var token = new PlayerSession
                {
                    Id = $"{username}/{tokenValue}",
                    Token = tokenValue,
                    Username = username,
                };
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
        }
    }
}
