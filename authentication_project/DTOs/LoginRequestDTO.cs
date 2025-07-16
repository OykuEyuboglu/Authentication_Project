using System.ComponentModel.DataAnnotations;

namespace authentication_project.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçersiz email formatı")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı")]
        public string? Password { get; set; }
    }
}

