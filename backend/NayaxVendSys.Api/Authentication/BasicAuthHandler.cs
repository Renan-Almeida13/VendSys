using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace NayaxVendSys.Api.Authentication;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string ValidUsername = "vendsys";
    private const string ValidPassword = "NFsZGmHAGWJSZ#RuvdiV";

    public BasicAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Authorization header not found.");
        }

        var authHeader = Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Basic scheme not found.");
        }

        var encodedCredentials = authHeader.Split(' ')[1];
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
        var parts = credentials.Split(':');

        if (parts.Length != 2 || parts[0] != ValidUsername || parts[1] != ValidPassword)
        {
            return AuthenticateResult.Fail("Invalid credentials.");
        }

        var claims = new[] { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, parts[0]) };
        var identity = new System.Security.Claims.ClaimsIdentity(claims, Scheme.Name);
        var principal = new System.Security.Claims.ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers["WWW-Authenticate"] = "Basic";
        Response.StatusCode = 401;
    }
} 