namespace Roulette.App.Model.Database
{
    public interface ISessionDatabase
    {
        public Task<PlayerSession?> GetSessionByTokenValue(string key);
        public Task<bool?> CreateSession(PlayerSession token);
        public Task<bool?> DeleteSession(string tokenValue);
        public PlayerSession? GenerateSessionToken(string username);
        public void CreateConnection();
        public void CloseConnection();
    }
}
