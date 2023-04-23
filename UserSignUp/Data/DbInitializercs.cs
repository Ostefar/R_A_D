using UserSignUp.Interfaces;
using UserSignUp.Models;

namespace UserSignUp.Data
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(SqlDbContext context)
        {
            // Delete the database, if it already exists. I do this because an
            // existing database may not be compatible with the entity model,
            // if the entity model was changed since the database was created.
            context.Database.EnsureDeleted();

            // Create the database, if it does not already exists. This operation
            // is necessary, if you dont't use the in-memory database.
            context.Database.EnsureCreated();

            // Look for any employees.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new List<User>
                {
                    new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "johndoe@example.com", Phone = 45454548, AddressLine1 = "123 Main St", City = "Anytown", ZipCode = "12345", Country = "USA" },
                    new User { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "janedoe@example.com", Phone = 45454545, AddressLine1 = "456 Elm St", City = "Otherville", ZipCode = "67890", Country = "USA" }
                };

        

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
