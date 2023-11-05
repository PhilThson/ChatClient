using System;
namespace BlazorServer.Data
{
	public class AuthResponse
	{
        public string? RefreshToken { get; set; }
        public string? JwtToken { get; set; }
    }
}

