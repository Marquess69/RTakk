using Microsoft.EntityFrameworkCore;
using RTakk.Models;
using System.Linq.Expressions;

namespace RTakk.Data.Interface
{
    public class UserRepo : IUserRepo
    {
        //DI
        private readonly ApplicationDbContext _context;
        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public User Create(User user)
        {
            _context.Users.Add(user);
            user.Id = _context.SaveChanges(); //returning id of created user

            return user;
        }

        public User GetByEmail(string email) 
        {
            return _context.Users!.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _context.Users!.FirstOrDefault(u => u.Id == id);
        }

        public List<User> GetAllUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }

        public User CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public List<User> GetUsersBySearchQuery(string searchQuery)
        {

            var users = _context.Users
                .Where(u => u.Username.Contains(searchQuery) || u.Email.Contains(searchQuery))
                .ToList();

            return users;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}
