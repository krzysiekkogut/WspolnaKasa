using Domain.Entities;

namespace DataAccessLayer.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string id);

        void UpdateUser(User user);

        void SaveChanges();
    }
}
