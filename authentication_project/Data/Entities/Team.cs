namespace authentication_project.Data.Entities;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // Takım lideri (User)
    public int LeaderId { get; set; }
    public User Leader { get; set; } = null!;

    // Üyeler (Users.TeamId = Team.Id)
    public ICollection<User> Members { get; set; } = new List<User>();
}
