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
        public string CreateToken(User user)
        {
            IEnumerable<Claim> claims = GetClaims(user);
            SigningCredentials credentials = new(ConstantsSecurity.Key, ConstantsSecurity.ALGORITHM);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = ConstantsSecurity.Exp(10, FormatExp.Minutes).ToUniversalTime(), //Expires requires the DateTime in UTC format.
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
                new(JwtRegisteredClaimNames.Name, user.FirsName + " " + user.LastName),
                new(JwtRegisteredClaimNames.Email, user.Email)
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
