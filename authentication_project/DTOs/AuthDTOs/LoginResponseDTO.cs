﻿namespace authentication_project.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string? Email { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }

    }
}