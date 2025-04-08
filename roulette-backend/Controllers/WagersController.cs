using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Roulette.App.Model;
using Roulette.App.Model.Database;
using Roulette.App.Model.DTO;

namespace Roulette.App.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WagersController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly ISessionDatabase _sessionContext;
        private readonly RouletteDbContext _databaseContext;

        public WagersController(ILogger<GamesController> logger, ISessionDatabase sessionDatabase,
            RouletteDbContext databaseContext)
        {
            _logger = logger;
            _sessionContext = sessionDatabase;
            _databaseContext = databaseContext;
        }

        [HttpPatch]
        [ActionName("UpdateWagerStatus")]
        public async Task<IActionResult> SavePlayerWagers([FromBody] UpdateWagerStatus status)
        {
            try
            {
                var AuthenticatedUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (AuthenticatedUser != status.Username) { return Forbid(); }
                var wagersToUpdate = new List<Wager>();
                foreach (var id in status.GameIds)
                {
                    if (!Guid.TryParse(id, out var gameId)) { return BadRequest(); }
                    var wager = await _databaseContext.Wagers.FirstOrDefaultAsync(wager => wager.GameId == gameId && wager.Username == status.Username);
                    if (wager != null)
                    {
                        wager.IncludedInBalance = true;
                    }
                }
                await _databaseContext.SaveChangesAsync();
                var token = new PlayerSession
                {
                    Token = Guid.NewGuid().ToString(),
                    Username = status.Username,
                };
                if (await _sessionContext.CreateSession(token) != true) { return StatusCode(503, "Service Unavailable"); }
                return Ok(new
                {
                    Token = token.Token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(503, "Service Unavailable");
            }

        }
    }
}
