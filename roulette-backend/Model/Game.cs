using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Roulette.App.Model
{
    public class Game
    {
        [NotMapped]
        public IBetCalculator betCalculator { get; set; }
        public Guid Id { get; set; }
        public Result Outcome { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Wager>? Wagers { get; set; }

        public bool? UpdateWinnings()
        {
            try
            {
                var numberUpdates = betCalculator.CalculateWinnings(Outcome, Wagers);
                return numberUpdates == Wagers.Count();
            }
            catch (Exception ex)
            {
                return null;
            }            
        }

    }
}
