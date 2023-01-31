using Microsoft.EntityFrameworkCore;

using DevPace.DB.Models;

namespace DevPace.DB
{
    public class DevPaceDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

        public DevPaceDbContext() { }

        public DevPaceDbContext(DbContextOptions<DevPaceDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User[]
            {
                new User
                {
                    Id = 1,
                    Name = "admin",
                    Password = "password"
                }
            });

            modelBuilder.Entity<Customer>().HasData(new Customer[]
            {
                new Customer
                {
                    Id = 1,
                    Name = "Stan",
                    CompanyName = "Poolz",
                    Email = "manager.poolz@gmail.com",
                    Phone = "+380631111111"
                }
            });
        }
    }
}
