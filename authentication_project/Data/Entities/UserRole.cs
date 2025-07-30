namespace authentication_project.Data.Entities
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
