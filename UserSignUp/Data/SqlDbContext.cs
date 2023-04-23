using Microsoft.EntityFrameworkCore;
using UserSignUp.Models;

namespace UserSignUp.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "johndoe@example.com", Phone = 78454596, AddressLine1 = "123 Main St", City = "Anytown", ZipCode = "12345", Country = "USA" },
                new User { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "janedoe@example.com", Phone = 45859652, AddressLine1 = "456 Elm St", City = "Otherville", ZipCode = "67890", Country = "USA" }
            );
        }
    }
}
