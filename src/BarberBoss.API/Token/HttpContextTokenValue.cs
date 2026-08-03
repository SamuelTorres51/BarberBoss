using BarberBoss.Domain.Security.Tokens;

namespace BarberBoss.API.Token;

public class HttpContextTokenValue : ITokenProvider {
    private readonly HttpContextAccessor _httpContext;

    public HttpContextTokenValue(HttpContextAccessor httpContext) {
        _httpContext = httpContext;
    }

    public string TokenOnRequest() {
        var authorization = _httpContext.HttpContext!.Request.Headers.Authorization.ToString();

        return 
    }
}
