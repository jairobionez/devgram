using System.Security.Claims;

namespace Devgram.Infra.Interfaces;

public interface IAspnetUser
{
    string Name { get; }
    string Action { get; }
    Guid? GetUserId();
    string GetUserRole();
    string GetUserEmail();
    bool IsAuthenticated();
    bool IsInRole(string role);
    IEnumerable<Claim> GetClaimsIdentity();
}