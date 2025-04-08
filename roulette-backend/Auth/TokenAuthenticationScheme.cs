using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Roulette.App.Model.Database;

namespace Roulette.App.Auth
{
    public class TokenAuthenticationScheme : AuthenticationHandler<TokenAuthOptions>
    {

        private readonly ISessionDatabase _sessionDb;


        public TokenAuthenticationScheme(IOptionsMonitor<TokenAuthOptions> options,
            ISessionDatabase db,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _sessionDb = db;           
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var encondedSessionTokenValue))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            }
            try
            {
                var decodedKey = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encondedSessionTokenValue));
                var token = (await _sessionDb.GetSessionByTokenValue(decodedKey))?.Token;
                if (token == null) { return await Task.FromResult(AuthenticateResult.Fail("Unauthorized - invalid credentials")); };                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, decodedKey.ToString().Split("/")[0])
                };
                var claimsIdentity = new ClaimsIdentity(claims, nameof(TokenAuthenticationScheme));
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);
                return await Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(AuthenticateResult.Fail("Service not available"));
            }
        }
    }
}
