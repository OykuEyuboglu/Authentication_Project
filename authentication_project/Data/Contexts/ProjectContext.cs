using Microsoft.EntityFrameworkCore;
using authentication_project.Data.Entities;

namespace authentication_project.Data.Contexts
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options)
          : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskCard> TaskCards { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Leader)
                .WithMany() // liderin kendi üyeleri Members ile gelmeyecek
                .HasForeignKey(t => t.LeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
               .HasOne(u => u.Team)
               .WithMany(t => t.Members)
               .HasForeignKey(u => u.TeamId)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.TeamId);
        }
    }
}
