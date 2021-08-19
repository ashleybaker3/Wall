using Microsoft.EntityFrameworkCore;

namespace Wall.Models
{
    public class WallContext : DbContext
    {
        public WallContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}