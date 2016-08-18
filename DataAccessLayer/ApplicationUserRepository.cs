using System.Data.Entity;
using DataAccessLayer.Entities;

namespace DataAccessLayer
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private ApplicationDbContext _dbContext;
        
        public ApplicationUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ApplicationUser GetUser(string id)
        {
            return _dbContext.Users.Find(id);
        }

        public void UpdateUser(ApplicationUser user)
        {
            _dbContext.Entry<ApplicationUser>(user).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
