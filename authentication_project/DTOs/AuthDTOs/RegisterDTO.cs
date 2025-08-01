using System.ComponentModel.DataAnnotations;

namespace authentication_project.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçersiz email formatı")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı")]
        public string? Password { get; set; }
        public int? RoleId { get; set; }
    }
}


