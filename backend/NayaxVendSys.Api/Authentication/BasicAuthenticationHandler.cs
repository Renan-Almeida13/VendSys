using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace NayaxVendSys.Api.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string REQUIRED_USERNAME = "vendsys";
    private const string REQUIRED_PASSWORD = "NFsZGmHAGWJSZ#RuvdiV";

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization scheme must be Basic"));
            }

            var encodedCredentials = authHeader["Basic ".Length..].Trim();
            var credentialBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

            if (credentials.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials format"));
            }

            var username = credentials[0];
            var password = credentials[1];

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));
            }

            if (username != REQUIRED_USERNAME || password != REQUIRED_PASSWORD)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header"));
        }
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers.Append("WWW-Authenticate", "Basic realm=\"VendSys API\", charset=\"UTF-8\"");
        return base.HandleChallengeAsync(properties);
    }
} 