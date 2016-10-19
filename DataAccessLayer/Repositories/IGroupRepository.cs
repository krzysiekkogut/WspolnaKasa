using Domain.Entities;
using System.Collections.Generic;

namespace DataAccessLayer.Repositories
{
    public interface IGroupRepository
    {
        void SaveChanges();

        IEnumerable<Group> GetAll(string userId);

        Group Get(int groupId);

        Group Get(string name);

        void Add(Group group);

        void Update(Group group);
        
        void Remove(Group group);

        bool Exists(string name);
    }
}
