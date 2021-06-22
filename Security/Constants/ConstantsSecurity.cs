using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Security.Constants
{
    public static class ConstantsSecurity
    {
        public const string KEYWORD_JWT = "¡my secr3t keYw0rd!";
        public const string ISS = @"https://localhost:44302/api/Users";
        public const string ALGORITHM = SecurityAlgorithms.HmacSha512;
        public static SymmetricSecurityKey Key => new(Encoding.UTF8.GetBytes(KEYWORD_JWT));
        public static Func<double, FormatExp, DateTime> Exp => static(add, format) => format switch
        {
            FormatExp.Minutes => DateTime.Now.AddMinutes(add),
            FormatExp.Hours => DateTime.Now.AddHours(add),
            FormatExp.Days => DateTime.Now.AddDays(add),
            FormatExp.NoExpiration or _ => DateTime.MaxValue,
        };
    }
}
