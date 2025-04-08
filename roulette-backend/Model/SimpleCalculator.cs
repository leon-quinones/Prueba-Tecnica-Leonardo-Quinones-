

namespace Roulette.App.Model
{
    public class SimpleCalculator : IBetCalculator
    {
        private readonly Dictionary <string, decimal> _multipliers = new Dictionary<string, decimal>();
        private readonly Dictionary<string, Func<Result, Result, bool>> _matchFunctions = new Dictionary<string, Func<Result, Result, bool>>();

        public SimpleCalculator()
        {
            _multipliers.Add("WagerOnlyColor", 0.5m);
            _multipliers.Add("WagerEvensColor", 1.0m);
            _multipliers.Add("WagerOddsColor", 1.0m);
            _multipliers.Add("WagerFull", 3.0m);

            _matchFunctions.Add("WagerOnlyColor", (outcome, betresult) => outcome.Color == betresult.Color);
            _matchFunctions.Add("WagerEvensColor", (outcome, betresult) => outcome.Color == betresult.Color && outcome.Number % 2 == 0);
            _matchFunctions.Add("WagerOddsColor", (outcome, betresult) => outcome.Color == betresult.Color && outcome.Number % 2 == 1);
            _matchFunctions.Add("WagerFull", (outcome, betresult) => outcome.Color == betresult.Color && outcome.Number == betresult.Number);
        }


        public int CalculateWinnings(Result gameOutCome, List<Wager> playerWagers)
        {
            int errors = 0;
            foreach (var wager in playerWagers)
            {
                try
                {
                    var winnings = calculator(gameOutCome, wager);
                    if (winnings == null) { errors++; continue; }
                    wager.Winnings = winnings;
                }
                catch (Exception ex)
                {
                   errors++;
                }
            }
            return playerWagers.Count() - errors;
        }

        private decimal? calculator(Result gameOutCome, Wager wager )
        { 
            if(!_multipliers.ContainsKey(wager.BetResult.Category.ToString())) { return null; }
            if(_matchFunctions[wager.BetResult.Category.ToString()](gameOutCome, wager.BetResult))
            {
                return _multipliers[wager.BetResult.Category.ToString()] * wager.Amount;
            } else
            {
                return -1m * wager.Amount;
            }
        }
    }

}
