using System.ComponentModel.DataAnnotations;

namespace Roulette.App.Model.DTO
{
    public class UpdatePlayerBalance
    {
        [Required(AllowEmptyStrings = false),
         MinLength(2, ErrorMessage = "Username must have at least {1} characters"),
         MaxLength(16, ErrorMessage = "characters and no more than {1} ")]
        public string Username { get; set; }
        [Required, Range(0, 1000000.0, ErrorMessage = "Account Balance must be between {1} and {2}")]
        public decimal Balance { get; set; }
    }
}
