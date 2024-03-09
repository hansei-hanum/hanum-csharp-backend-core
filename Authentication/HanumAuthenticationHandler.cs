using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Hanum.Core.Protos.Auth;

namespace Hanum.Core.Authentication;

public class HanumAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration,
    AuthService.AuthServiceClient authServiceClient) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder) {
    public const string SchemeName = "HanumCommonAuth";

    private readonly bool _bypassAuth = configuration.GetValue("Hanum:BypassAuth", false);

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
        var token = Request.Headers.Authorization.ToString().Split(" ");

        if (token.Length != 2 || token[0] != "Bearer")
            return AuthenticateResult.Fail("Token is missing");

        string? userId = null;

        if (!_bypassAuth) {
            var response = await authServiceClient.AuthorizeAsync(
                new AuthorizeRequest { Token = token[1] });

            if (response.Success || response.HasUserid)
                userId = response.Userid.ToString();
        } else {
            userId = token[1];

            if (!ulong.TryParse(userId, out var _))
                userId = null;
        }

        if (string.IsNullOrEmpty(userId))
            return AuthenticateResult.Fail("Token is invalid");

        return AuthenticateResult.Success(new(
            new(
                new ClaimsIdentity(
                    [
                        new(ClaimTypes.NameIdentifier, userId)
                    ],
                    Scheme.Name
                )
            ),
            Scheme.Name
        ));
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class HanumCommomAuthorizeAttribute : AuthorizeAttribute {
    public HanumCommomAuthorizeAttribute() {
        AuthenticationSchemes = HanumAuthenticationHandler.SchemeName;
    }
}

public static class HanumAuthenticationHandlerExtensions {
    public static AuthenticationBuilder AddHanumCommonAuthScheme(this AuthenticationBuilder authenticationBuilder) =>
        authenticationBuilder.AddScheme<AuthenticationSchemeOptions, HanumAuthenticationHandler>(HanumAuthenticationHandler.SchemeName, null);
}