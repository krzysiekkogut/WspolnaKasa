using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Services
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
