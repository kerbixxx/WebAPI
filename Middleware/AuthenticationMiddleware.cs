using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SimbirSoft.Repositories.Interfaces;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace SimbirSoft.Middleware
{
    public class ApiKeySchemeOptions : AuthenticationSchemeOptions
    {
        public string HeaderName { get; set; }
        public string? ApiKey { get; set; }
        public bool ReadOnly { get; set; } = true;

        public override void Validate()
        {
            if (string.IsNullOrEmpty(HeaderName))
            {
                throw new ArgumentException("Header name must be provided.");
            }

            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new ArgumentException("API key must be provided.");
            }
        }
    }

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAccountRepository _accountRepository;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAccountRepository accountRepository)
            : base(options, logger, encoder, clock)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeader = Context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            var base64Credentials = authorizationHeader.Substring("Basic ".Length).Trim();
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(base64Credentials)).Split(':');

            if (credentials.Length != 2)
            {
                return AuthenticateResult.Fail("Invalid Basic authentication header format.");
            }

            var user = _accountRepository.FirstOrDefault(u => u.email == credentials[0]);
            if (user == null || user.password != credentials[1])
            {
                return AuthenticateResult.Fail("Invalid username or password.");
            }

            var claims = new[] { new Claim(ClaimTypes.Name, user.email) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }

}
