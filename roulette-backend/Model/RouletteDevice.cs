namespace Roulette.App.Model
{
    public class RouletteDevice : IRoulette
    {
        private readonly ITableLayout _tableLayout;
        private readonly Random _random;
        public RouletteDevice(ITableLayout tableLayout)
        {
            _tableLayout = tableLayout;
            _random = new Random();
        }
        public Result SimulateGame()
        {
            var randomIndex = _random.Next(_tableLayout.GetNumbers().Count());
            var winningNumber = _tableLayout.GetNumbers()[randomIndex];
            var winningColor = _tableLayout.GetColor(winningNumber);
            return new Result { Color=winningColor, Number=winningNumber};
        }
    }
}
