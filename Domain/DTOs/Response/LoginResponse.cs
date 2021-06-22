using System;

namespace Domain.DTOs.Response
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
