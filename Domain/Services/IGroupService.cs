using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface IGroupService
    {
        IEnumerable<Group> GetAllGroups(string userId);

        Group GetGroup(int groupId);

        void CreateGroup(string groupName, string secret, string currentUserId);

        void JoinGroup(string groupName, string secret, string currentUserId);

        void EditGroup(int groupId, string newGroupName);

        void RemoveGroup(int groupId, string secret);
    }
}
