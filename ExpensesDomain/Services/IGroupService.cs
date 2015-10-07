using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Entities.ExpensesDomain;

namespace ExpensesDomain.Services
{
    public interface IGroupService
    {
        IEnumerable<Group> GetAllGroups(string userId);

        Group GetGroup(int groupId);

        bool CreateGroup(string name, string secret, string currentUser);

        void JoinGroup(string name, string secret, string currentUser);

        bool EditGroup(int groupId, string newName);

        bool RemoveGroup(int groupId, string secret);
    }
}
