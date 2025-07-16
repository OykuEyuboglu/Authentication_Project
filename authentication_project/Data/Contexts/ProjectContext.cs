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
        }
    }
