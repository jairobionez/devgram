using System.Security.Claims;

namespace Devgram.Data.Interfaces;

public interface IAspnetUser
{
    string Name { get; }
    string Action { get; }
    bool Admin();
    Guid? GetUserId();
    string GetUserRole();
    string GetUserEmail();
    bool IsAuthenticated();
    bool IsInRole(string role);
    IEnumerable<Claim> GetClaimsIdentity();
}