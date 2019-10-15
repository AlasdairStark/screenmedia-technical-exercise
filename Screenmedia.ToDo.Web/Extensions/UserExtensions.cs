using System.Security.Claims;

namespace Screenmedia.ToDo.Web.Extensions
{
    public static class UserExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
