using System.ComponentModel.DataAnnotations;

namespace Roulette.App.Model.Database
{
    public class SessionDatabaseConfig
    {
        [Required]
        public string Host { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public string DatabaseName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string SessionDbName { get; set; }
        [Required]
        public int SessionTimeout { get; set; }
    }
}