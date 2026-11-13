// Owner: Reyes | Type: static class (extension methods for the logged-in user)
using System.Security.Claims;

namespace GymApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>Id of the logged-in user, read from the JWT (never from the URL or body).</summary>
    public static int GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(value, out var id) ? id : 0;
    }

    public static string GetRole(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
    }
}
