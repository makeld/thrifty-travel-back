using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WebApp.Helpers;

public class WebUserNameProvider : IUserNameProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WebUserNameProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserName()
    {
        if (_httpContextAccessor.HttpContext == null) return "System";
        var userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email) ?? "Anonymous";
        return userEmail;
    }
}