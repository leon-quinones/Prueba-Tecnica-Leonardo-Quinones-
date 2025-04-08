namespace Roulette.App.Model
{
    public interface IBetCalculator
    {
        public int CalculateWinnings(Result gameOutCome, List<Wager> playerWagers);
    }
}
