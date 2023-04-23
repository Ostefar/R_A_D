using Microsoft.EntityFrameworkCore;
using UserSignUp.Models;

namespace UserSignUp.Data
{
    public class TestData
    {
        public void SeedData(SqlDbContext dbContext)
        {
            var users = new List<User>
                {
                    new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "johndoe@example.com", Phone = 45454548, AddressLine1 = "123 Main St", City = "Anytown", ZipCode = "12345", Country = "USA" },
                    new User { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "janedoe@example.com", Phone = 45454545, AddressLine1 = "456 Elm St", City = "Otherville", ZipCode = "67890", Country = "USA" }
                };

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }
    }
}
