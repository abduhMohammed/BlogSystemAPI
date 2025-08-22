using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogSystemAPI.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public AppDbContext() { }

        public DbSet<BlogPost> blogPosts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Comment> comments { get; set; }

        
        /*
         * protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=HP;Database=BlogSystemDB;Trusted_Connection=True;Trust Server Certificate=True;");
            }
        }
        */
        
    }
}