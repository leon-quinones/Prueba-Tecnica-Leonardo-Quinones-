namespace Roulette.App.Model
{
    public class Wager
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public PlayerAccount PlayerAccount { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
        public Result BetResult { get; set; }
        public Decimal Amount { get; set; }
        public Decimal? Winnings { get; set; }
        public bool? IncludedInBalance { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
