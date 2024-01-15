using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using System.Security.Principal;
using System.Text;

namespace SimbirSoft.Middleware
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly IAccountRepository _accountRepository;

        public AuthenticationHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var credentials = ParseAuthorizationHeader(request);

            if (credentials != null)
            {
                var user = _accountRepository.FirstOrDefault(x => x.email == credentials.email);
                if (user != null && user.password == credentials.password)
                {
                    var identity = new GenericIdentity(credentials.email);
                    Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
                }
            }
            return base.SendAsync(request, cancellationToken);
        }

        protected virtual Credentials ParseAuthorizationHeader(HttpRequestMessage request)
        {
            string authorizationHeader = null;
            var authorization = request.Headers.Authorization;
            if (authorization != null && authorization.Scheme == "Basic")
                authorizationHeader = authorization.Parameter;

            if (string.IsNullOrEmpty(authorizationHeader))
                return null;

            authorizationHeader = Encoding.Default.GetString(Convert.FromBase64String(authorizationHeader));

            var authenticationTokens = authorizationHeader.Split(':');
            if (authenticationTokens.Length < 2)
                return null;

            return new Credentials() { email = authenticationTokens[0], password = authenticationTokens[1], };
        }
    }
}
