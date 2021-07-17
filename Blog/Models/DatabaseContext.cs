using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Models {
    public class DatabaseContext : DbContext {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Filename=BlogDB.db");
            optionsBuilder.UseSqlite("Filename=UserProfileDB.db");
        }
        public DbSet<Post> Post { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
    }
}
