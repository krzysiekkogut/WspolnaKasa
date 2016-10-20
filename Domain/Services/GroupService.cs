using DataAccessLayer;
using Domain.Entities;
using Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services
{
    public class GroupService : IGroupService
    {
        private IUnitOfWork _unitOfWork;

        public GroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Group> GetAllGroups(string userId)
        {
            return _unitOfWork.UsersRepository.Get(userId).Groups;
        }

        public Group GetGroup(int groupId)
        {
            return _unitOfWork.GroupsRepository.Get(groupId);
        }

        public bool CreateGroup(string groupName, string secret, string currentUserId)
        {
            if (GroupExists(groupName))
            {
                return false;
            }

            var group = new Group
            {
                Name = groupName,
                Secret = secret
            };
            group.Members.Add(_unitOfWork.UsersRepository.Get(currentUserId));
            _unitOfWork.GroupsRepository.Add(group);
            _unitOfWork.SaveChanges();
            
            return true;
        }

        public void JoinGroup(string groupName, string secret, string currentUserId)
        {
            if (!GroupExists(groupName))
            {
                throw new GroupNotFoundException();
            }

            var group = _unitOfWork.GroupsRepository.GetAll().Single(g => g.Name == groupName);
            if (group.Secret != secret)
            {
                throw new WrongGroupPasswordException();
            }

            group.Members.Add(_unitOfWork.UsersRepository.Get(currentUserId));
            _unitOfWork.GroupsRepository.Update(group);
            _unitOfWork.SaveChanges();
        }
        
        public bool EditGroup(int groupId, string newGroupName)
        {
            if (GroupExists(newGroupName))
            {
                return false;
            }

            var group = _unitOfWork.GroupsRepository.Get(groupId);
            group.Name = newGroupName;
            _unitOfWork.GroupsRepository.Update(group);
            _unitOfWork.SaveChanges();
            return true;
        }

        public bool RemoveGroup(int groupId, string secret)
        {
            var group = _unitOfWork.GroupsRepository.Get(groupId);
            if (group.Secret != secret)
            {
                return false;
            }

            _unitOfWork.GroupsRepository.Remove(group);
            _unitOfWork.SaveChanges();
            return true;
        }

        private bool GroupExists(string name)
        {
            return _unitOfWork.GroupsRepository
                .GetAll().Any(g => g.Name == name);
        }
    }
}
