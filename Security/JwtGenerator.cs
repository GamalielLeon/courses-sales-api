using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using Security.Constants;
using Security.Contracts;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Security
{
    public class JwtGenerator : IJwtGenerator
    {
        public string CreateToken(User user, string[] roles = null)
        {
            IList<Claim> claims = GetClaims(user).ToList();
            SigningCredentials credentials = new(ConstantsSecurity.Key, ConstantsSecurity.ALGORITHM);

            // Add role codes to the "role" claim, as an array.
            if (roles?.Length > 0)
                claims = roles.Aggregate(claims, (claims, role) => { claims.Add(new Claim(ClaimTypes.Role, role)); return claims; });

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = ConstantsSecurity.Exp(1, FormatExp.Hours).ToUniversalTime(), //Expires requires the DateTime in UTC format.
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescription));
        }

        public IEnumerable<Claim> GetClaims(User user)
        {
            return new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Iss, ConstantsSecurity.ISS),
                new(JwtRegisteredClaimNames.Name, user.FirstName + " " + user.LastName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Sub, ConstantsSecurity.SUB)
            };
        }

        public string GetEmailFromToken(string token)
        {
            const string emailClaim = JwtRegisteredClaimNames.Email;
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ReadJwtToken(token).Payload.Claims.FirstOrDefault(static c => c.Type == emailClaim).Value;
        }
    }
}
