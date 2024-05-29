using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STEtest.Models;

namespace STEtest.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserProfile>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Admin>()
                .HasOne(a => a.UserProfile)
                .WithMany()
                .HasForeignKey(a => a.UserProfileId)
                .IsRequired();

            builder.Entity<Student>()
                .HasOne(s => s.UserProfile)
                .WithMany()
                .HasForeignKey(s => s.UserProfileId)
                .IsRequired();
        }
    }
}
