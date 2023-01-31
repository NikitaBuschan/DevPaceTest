using System.Text;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

using DevPace.DB;

namespace DevPace.Handler
{
    // Authorization | base YWRtaW46cGFzc3dvcmQ=

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DevPaceDbContext _context;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> option,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            DevPaceDbContext context) : base(option, logger, encoder, clock)
        {
            _context = context;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("No header found");
            }

            var headervalue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

            var bytes = Convert.FromBase64String(headervalue.Parameter);

            string credentials = Encoding.UTF8.GetString(bytes);

            if (!string.IsNullOrEmpty(credentials))
            {
                string[] array = credentials.Split(":");

                string username = array[0];
                string password = array[1];

                var user = _context.Users.FirstOrDefault(user => user.Name == username && user.Password == password);

                if (user == null)
                {
                    return AuthenticateResult.Fail("UnAuthorized");
                }

                var claim = new[] { new Claim(ClaimTypes.Name, username) };

                var identity = new ClaimsIdentity(claim, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult.Fail("UnAuthorized");
            }
        }
    }
}
