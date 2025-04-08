using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roulette.App.Model;
using Roulette.App.Model.Database;
using Roulette.App.Model.DTO;

namespace Roulette.App.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly ISessionDatabase _sessionContext;
        private readonly RouletteDbContext _databaseContext;

        public GamesController(ILogger<GamesController> logger, ISessionDatabase sessionDatabase,
            RouletteDbContext databaseContext)
        {
            _logger = logger;
            _sessionContext = sessionDatabase;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [Route("{id}/Wagers", Name = "GetWagerByGameBy")]
        public async Task<IActionResult> GetWagers(string id, [FromQuery] string player)
        {
            try
            {
                if (!Guid.TryParse(id, out var gameId) || string.IsNullOrWhiteSpace(player)) { return BadRequest(); }
                var AuthenticatedUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (player != AuthenticatedUser) { return Forbid(); }
                var game = _databaseContext.Wagers.FirstOrDefault(wager => wager.GameId == gameId && wager.Username == player);
                if (game == null) { return NotFound(); }
                var token = new PlayerSession
                {
                    Token = Guid.NewGuid().ToString(),
                    Username = player,
                };
                if (await _sessionContext.CreateSession(token) != true) { return StatusCode(503, "Service Unavailable"); }
                return Ok(new
                {                 
                    Token = token.Token,
                    Player = game.Username,
                    Winnings = game.Winnings
                });
            }
            catch (Exception ex)
            {
                return StatusCode(503, "Service Unvailable");
            }
        }


        [HttpPost]        
        public async Task<IActionResult> CreateGame([FromBody] CreateGame gameData)
        {
            try
            {
                if (gameData == null) { return BadRequest(); }
                var AuthenticatedUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (gameData.Username != AuthenticatedUser) { return Forbid(); }
                RouletteDevice roulette = new RouletteDevice(new ThirtySixTable());
                var gameResult = roulette.SimulateGame();
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Wagers = new List<Wager>(),
                    Outcome = gameResult,
                };
                game.betCalculator = new SimpleCalculator();
                var playerWager = new Wager()
                {
                    Id = Guid.NewGuid(),
                    Username = gameData.Username,
                    GameId = game.Id,
                    Game = game,
                    Amount = gameData.Amount,
                    BetResult = new Result
                    {
                        Id = Guid.NewGuid(),
                        Color = gameData.Color,
                        Number = gameData.Number,
                        Category = (ResultCategory)Enum.Parse(typeof(ResultCategory), gameData.BetType)
                    },
                    
                };
                game.Wagers.Add(playerWager);                
                var areWinningsUpdate = game.UpdateWinnings();
                _databaseContext.Games.Add(game);
                _databaseContext.Wagers.Add(playerWager);
                await _databaseContext.SaveChangesAsync();
                if (areWinningsUpdate != true) { return StatusCode(503, "Service Unvailable"); }
                var token = new PlayerSession
                {
                    Token = Guid.NewGuid().ToString(),
                    Username = gameData.Username,
                };
                if (await _sessionContext.CreateSession(token) != true) { return StatusCode(503, "Service Unavailable"); }
                return Created("", new { 
                    Token = token.Token,
                    GameId = game.Id,
                    Number = game.Outcome.Number,
                    Color = game.Outcome.Color
                });
            }
            catch (Exception ex)
            {
                return StatusCode(503, "Service Unvailable");
            }

        }

    }
}
