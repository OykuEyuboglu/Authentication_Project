namespace authentication_project.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string EmployeeId { get; set; }
        public string Title { get; set; }
        public string PasswordHash { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }

    }
}