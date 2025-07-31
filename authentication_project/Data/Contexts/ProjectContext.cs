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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
        }
    }
}
