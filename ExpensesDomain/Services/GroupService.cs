using System.Collections.Generic;
using DataAccessLayer;
using DataAccessLayer.Entities.ExpensesDomain;
using ExpensesDomain.Exceptions;
using ExpensesDomain.Repositories;

namespace ExpensesDomain.Services
{
    public class GroupService : IGroupService
    {
        private IGroupRepository _groupRepository;
        private IApplicationUserRepository _usersRepository;

        public GroupService(IGroupRepository groupRepository, IApplicationUserRepository usersRepository)
        {
            _groupRepository = groupRepository;
            _usersRepository = usersRepository;
        }

        public IEnumerable<Group> GetAllGroups(string userId)
        {
            return _groupRepository.GetAll(userId);
        }

        public Group GetGroup(int groupId)
        {
            return _groupRepository.Get(groupId);
        }

        public bool CreateGroup(string name, string secret, string currentUser)
        {
            if (GroupExists(name)) return false;
            var group = new Group
            {
                Name = name,
                Secret = secret
            };
            group.Members.Add(_usersRepository.GetUser(currentUser));
            _groupRepository.Add(group);
            _groupRepository.SaveChanges();
            
            return true;
        }

        public void JoinGroup(string name, string secret, string currentUser)
        {
            if (!GroupExists(name))
            {
                throw new GroupNotFoundException();
            }
            var group = _groupRepository.Get(name);
            if (group.Secret != secret)
            {
                throw new WrongGroupPasswordException();
            }

            group.Members.Add(_usersRepository.GetUser(currentUser));
            _groupRepository.Update(group);
            _groupRepository.SaveChanges();
        }
        
        public bool EditGroup(int groupId, string newName)
        {
            if (GroupExists(newName)) return false;
            var group = _groupRepository.Get(groupId);
            group.Name = newName;
            _groupRepository.Update(group);
            _groupRepository.SaveChanges();
            return true;
        }

        public bool RemoveGroup(int groupId, string secret)
        {
            var group = _groupRepository.Get(groupId);
            if (group.Secret != secret) return false;
            _groupRepository.Remove(group);
            _groupRepository.SaveChanges();
            return true;
        }
     
        private bool GroupExists(string name)
        {
            return _groupRepository.Exists(name);
        }
    }
}
