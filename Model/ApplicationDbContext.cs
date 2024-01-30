using Microsoft.EntityFrameworkCore;

namespace myappdotnet.Model
{
    public class ApplicationDbContext : DbContext
    {
        
        public DbSet<MyUser> MyUser { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Activities> Activities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
    
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MyUser>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Location>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Activities>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
        }
    }
}