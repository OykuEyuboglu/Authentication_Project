namespace authentication_project.DTOs.Auth
{
    public class UserProfilDTO
    {
        public string Email {  get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string RoleDescription { get; set; }
    }
}
