using global::TaskManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Task = TaskManagementAPI.Models.Task;

namespace TaskManagementAPI.Data
{
    namespace TaskManagementAPI.Data
    {
        public class ApplicationDbContext : DbContext
        {
            public DbSet<User> Users { get; set; }
            public DbSet<Task> Tasks { get; set; }

            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
                modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

                modelBuilder.Entity<Task>()
                    .HasOne(t => t.User)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.UserId);

                base.OnModelCreating(modelBuilder);
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("YourConnectionString",
                    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
            }
        }
    }

}
