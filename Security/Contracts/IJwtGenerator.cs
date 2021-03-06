using Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace Security.Contracts
{
    public interface IJwtGenerator
    {
        string CreateToken(User user, string[] roles = null);
        IEnumerable<Claim> GetClaims(User user);
        string GetEmailFromToken(string token);
    }
}
