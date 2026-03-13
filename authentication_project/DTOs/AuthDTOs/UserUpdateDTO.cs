using System.ComponentModel.DataAnnotations;

namespace authentication_project.DTOs.AuthDTOs
{
    public class UserUpdateDTO
    {
        public string? Email { get; set; }

        public string? Username { get; set; }

        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı")]
        public string? PasswordHash { get; set; }
        public int? RoleId { get; set; }
    }
}
