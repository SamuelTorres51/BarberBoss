using BarberBoss.Domain.Security.Tokens;

namespace BarberBoss.API.Token;

public class HttpContextTokenValue : ITokenProvider {
    private readonly IHttpContextAccessor _httpContext;

    public HttpContextTokenValue(IHttpContextAccessor httpContext) {
        _httpContext = httpContext;
    }

    public string TokenOnRequest() {
        var authorization = _httpContext.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization["Bearer ".Length..].Trim();
    }
}
