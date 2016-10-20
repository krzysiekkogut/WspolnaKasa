using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface IGroupService
    {
        IEnumerable<Group> GetAllGroups(string userId);

        Group GetGroup(int groupId);

        bool CreateGroup(string groupName, string secret, string currentUserId);

        void JoinGroup(string groupName, string secret, string currentUserId);

        bool EditGroup(int groupId, string newGroupName);

        bool RemoveGroup(int groupId, string secret);
    }
}
