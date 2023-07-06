using RTakk.Models;

namespace RTakk.Data.Interface
{
    public interface IUserRepo
    {
        User Create(User user);
        User GetByEmail(string email);
        User GetById (int id);
        List<User> GetAllUsers();
        User CreateUser(User user);
        void DeleteUser(int userId);
        List<User> GetUsersBySearchQuery(string searchQuery);
        IEnumerable<User> GetUsers();
    }
}
