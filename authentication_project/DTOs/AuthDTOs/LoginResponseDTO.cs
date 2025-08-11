namespace authentication_project.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string Role { get; set; }

    }
}