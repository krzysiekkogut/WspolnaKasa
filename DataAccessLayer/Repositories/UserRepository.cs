using Domain.Entities;
using System.Data.Entity;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _dbContext;
        
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetUser(string id)
        {
            return _dbContext.Users.Find(id);
        }

        public void UpdateUser(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
