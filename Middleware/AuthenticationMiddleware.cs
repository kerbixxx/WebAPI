using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace SimbirSoft.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAccountRepository accountRepository)
        {
            var credentials = ParseAuthorizationHeader(context.Request);

            if (credentials != null)
            {
                var user = accountRepository.FirstOrDefault(x => x.email == credentials.email);
                if (user != null && user.password == credentials.password)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.email) };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    context.User = new ClaimsPrincipal(identity);
                }
            }

            await _next(context);
        }

        protected virtual Credentials ParseAuthorizationHeader(HttpRequest request)
        {
            string authorizationHeader = null;
            var authorization = request.Headers["Authorization"];

            if(string.IsNullOrEmpty(authorizationHeader))
            {
                return null;
            }
            if (authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                var encodedCredentials = authorizationHeader["Basic ".Length..].Trim();
                var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

                var credentialsParts = decodedCredentials.Split(':');
                if (credentialsParts.Length == 2)
                {
                    return new Credentials { email = credentialsParts[0], password = credentialsParts[1] };
                }
            }

            return null;
        }
    }
}
