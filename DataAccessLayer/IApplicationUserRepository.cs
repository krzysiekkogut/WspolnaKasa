using DataAccessLayer.Entities;

namespace DataAccessLayer
{
    public interface IApplicationUserRepository
    {
        ApplicationUser GetUser(string id);

        void UpdateUser(ApplicationUser user);

        void SaveChanges();
    }
}
