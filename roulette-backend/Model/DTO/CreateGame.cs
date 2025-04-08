using System.ComponentModel.DataAnnotations;

namespace Roulette.App.Model.DTO
{
    public class CreateGame
    {
        [Required(AllowEmptyStrings = false), 
         MinLength(2, ErrorMessage = "Username must have at least {1} characters"), 
         MaxLength(16, ErrorMessage = "characters and no more than {1} ")]
        public string Username { get; set; }
        [Required, EnumDataType(typeof(ResultCategory), ErrorMessage = "Invalid Bet's Category")]
        public string BetType { get; set; }
        [Required, Range(1.0, 1000000.0, ErrorMessage = "Wager amount must be between {1} and {2}")]
        public decimal Amount { get; set; }
        [Required, Range(-1, 36, ErrorMessage = "Invalid bet number")]
        public int Number { get; set; }
        [Required, EnumDataType(typeof(Colors), ErrorMessage = "Invalid Bet Color Option")]
        public string Color { get; set; }

    }

    public enum Colors
    {
        Red,
        Black,
        Green
    }
}
