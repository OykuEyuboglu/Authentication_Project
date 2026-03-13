namespace authentication_project.DTOs.AuthDTOs
{
    public class CreateUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmployeeId { get; set; }
        public string Title { get; set; }
        public string AppLanguage { get; set; }
    }
}
