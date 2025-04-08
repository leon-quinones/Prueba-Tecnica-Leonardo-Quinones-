using System.ComponentModel.DataAnnotations;

namespace Roulette.App.Model
{
    public class PlayerSession
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string Username { get; set; }
    }

}
