
namespace Roulette.App.Model
{
    public class PlayerAccount
    {
        public string Username { get; set; }
        public Decimal Balance { get; set; }
        public List<Wager>? Wagers { get; set; }
    }
}
