namespace Roulette.App.Model.Database
{
    public abstract class AbstractDatabaseHandler
    {
        public bool IsConnected { get; set; }
        public bool IsClosed { get; set; }
        public abstract void CreateConnection();
        public abstract void CloseConnection();

    }
}
