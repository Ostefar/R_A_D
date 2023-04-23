using Microsoft.AspNetCore.DataProtection.Repositories;
using UserSignUp.Data;
using UserSignUp.Interfaces;
using UserSignUp.Models;

namespace UserSignUp.Repositories
{
    public class UserRepo : IRepo<User>
    {

        private readonly SqlDbContext db;

        public UserRepo(SqlDbContext context)
        {
            db = context;
        }

        public void Add(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
        }

        public void Edit(User entity)
        {
            throw new NotImplementedException();
        }

        public User Get(int id)
        {
            return db.Users.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users.ToList();
        }

        public void Remove(int id)
        {

            var users = db.Users.Single(r => r.Id == id);
            db.Users.Remove(users);
            db.SaveChanges();
        }
    }
}
