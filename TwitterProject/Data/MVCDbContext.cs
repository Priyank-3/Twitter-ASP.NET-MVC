using Microsoft.EntityFrameworkCore;
using TwitterProject.Models.Domain;

namespace TwitterProject.Data
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tweet> Tweets { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
