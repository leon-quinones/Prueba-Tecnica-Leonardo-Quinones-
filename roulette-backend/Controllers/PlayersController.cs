using System.Security.Claims;
using System.Text.RegularExpressions;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly ISessionDatabase _sessionContext;
        private readonly RouletteDbContext _databaseContext;

        public PlayersController(ILogger<GamesController> logger, ISessionDatabase sessionDatabase,
            RouletteDbContext databaseContext)
        {
            _logger = logger;
            _sessionContext = sessionDatabase;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [Route("{id}", Name = "GetPlayerBalance")]
        public async Task<IActionResult> GetPlayerBalance(string id)
        {
            try
            {
                var AuthenticatedUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (AuthenticatedUser != id) { return Forbid(); }
                var user = _databaseContext.Players.FirstOrDefault(user => user.Username == id);
                if (user == null) { return NotFound(); }
                var userWinnings = await _databaseContext.Wagers
                    .Where(wager => wager.Username == id && wager.IncludedInBalance == true)
                    .SumAsync(wager => wager.Winnings);
                if (userWinnings == null) { userWinnings = 0m; }
                var token = _sessionContext.GenerateSessionToken(user.Username);
                if (token == null) { return StatusCode(503, "Service Unavailable"); }
                if (await _sessionContext.CreateSession(token) != true) { return StatusCode(503, "Service Unavailable"); }
                return Ok(new
                {
                    token = token.Token,   
                    Balance = user.Balance  + userWinnings,
                });
            } catch (Exception ex)
            {
                return StatusCode(503, "Service Unavailable");
            }
        }
        
        [HttpPost]
        [Route("{id}", Name = "UpdatePlayerBalance")]
        public async Task<IActionResult> UpdatePlayerBalance(string id, [FromBody] UpdatePlayerBalance account)
        {
            try
            {
                var AuthenticatedUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (AuthenticatedUser != id) { return Forbid(); }
                var user = await _databaseContext.Players.FirstOrDefaultAsync(user => user.Username == id);
                if (user == null) { return NotFound("User was not found"); }
                user.Balance = user.Balance + account.Balance;
                user.Wagers = new List<Wager>();               
                await _databaseContext.SaveChangesAsync();
                var token = _sessionContext.GenerateSessionToken(user.Username);
                if (token == null) { return StatusCode(503, "Service Unavailable"); }
                if (await _sessionContext.CreateSession(token) != true) { return StatusCode(503, "Service Unavailable"); }
                return Created("token", new { Token = token.Token });

            } catch (Exception ex)
            {
                return StatusCode(503, "Service Unavailable");
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp", Name = "SignupPlayer")]
        public async Task<IActionResult> Signup([FromBody] SignupPlayer account)
        {
            try
            {
                var username = Regex.Replace(account.Username, "[A-Za-z]", m => m.Value.ToLower());               
                var user = await _databaseContext.Players.FirstOrDefaultAsync(user => user.Username == username);
                var tokenValue = Guid.NewGuid().ToString();
                var token = _sessionContext.GenerateSessionToken(username);
                if (token == null) { return StatusCode(503, "Service Unavailable"); }
                if (await _sessionContext.CreateSession(token) != true) { return StatusCode(503, "Service Unavailable"); }
                if (user != null) { 
                    return Conflict(new
                    {
                        Token = token.Token,
                        mesage= "Username already registered",
                    }); 
                }
                user = new PlayerAccount
                {
                    Username = username,
                    Balance = 0,
                    Wagers = new List<Wager>()
                };
                _databaseContext.Players.Add(user);
                await _databaseContext.SaveChangesAsync();

                
                return Created("token", new {Token = token.Token});
            }
            catch (Exception ex)
            {
                return StatusCode(503, "Service Unavailable");
            }
        }


    }
}
