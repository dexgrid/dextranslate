using DexTranslate.Api.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DexTranslate.Api.Filters
{
    internal class TokenHandler : AuthenticationHandler<CustomAuthenticationOptions>
    {
        private readonly IOptionsMonitor<CustomAuthenticationOptions> _options;

        public TokenHandler(IOptionsMonitor<CustomAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _options = options;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.Request.Headers.TryGetValue("ApiKey", out var keys)
                && Context.Request.Headers.TryGetValue("ApiSecret", out var secrets))
            {
                var key = keys.FirstOrDefault();
                var secret = secrets.FirstOrDefault();
                var options = _options.CurrentValue;

                if (options.ApiKey == key && options.ApiSecret == secret)
                {
                    var identity = new ClaimsIdentity("api");
                    var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), null, "api");
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }

                return Task.FromResult(AuthenticateResult.Fail("Incorrect'ApiKey' or 'ApiSecret' header."));
            }

            return Task.FromResult(AuthenticateResult.Fail("Incorrect'ApiKey' or 'ApiSecret' header."));
        }
    }
}