using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Entities.ExpensesDomain;

namespace ExpensesDomain.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private ApplicationDbContext _dbContext;
        public GroupRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public IEnumerable<Group> GetAll(string userId)
        {
            return _dbContext.Users.Find(userId).Groups;
        }

        public Group Get(string name)
        {
            return _dbContext.Set<Group>().First(g => g.Name == name);
        }

        public Group Get(int groupId)
        {
            return _dbContext.Set<Group>().Find(groupId);
        }

        public void Add(Group group)
        {
            _dbContext.Set<Group>().Add(group);
        }

        public void Update(Group group)
        {
            _dbContext.Entry<Group>(group).State = EntityState.Modified;
        }

        public void Remove(Group group)
        {
            _dbContext.Set<Group>().Remove(group);
        }
        
        public bool Exists(string name)
        {
            return _dbContext.Set<Group>().Any(g => g.Name == name);
        }
    }
}
