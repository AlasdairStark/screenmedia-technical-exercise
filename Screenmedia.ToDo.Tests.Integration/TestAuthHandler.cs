using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Screenmedia.ToDo.Tests.Integration
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "9d253132-0e1c-4787-bd51-2d0c6eb23855") };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
