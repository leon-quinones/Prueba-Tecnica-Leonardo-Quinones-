using System.ComponentModel.DataAnnotations;

namespace Roulette.App.Model.DTO
{
    public class UpdateWagerStatus
    {
        [Required(AllowEmptyStrings = false),
         MinLength(2, ErrorMessage = "Username must have at least {1} characters"),
         MaxLength(16, ErrorMessage = "characters and no more than {1} ")]
        public string Username { get; set; }
        [Required, MinLength(0, ErrorMessage= "Invalid number of games to store")]
        public List<string> GameIds { get; set; }
    }
}
