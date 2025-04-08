namespace Roulette.App.Model
{
    public class Result
    {
        public Guid Id { get; set; }
        public int? Number { get; set; }
        public string Color { get; set; }
        public ResultCategory Category { get; set; }
    }

    public enum ResultCategory
    {
        Game,
        WagerOnlyColor,
        WagerEvensColor,
        WagerOddsColor,
        WagerFull
    }
}
