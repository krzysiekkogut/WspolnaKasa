using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Services;
using Domain.Entities;
using Domain.Exceptions;
using DataAccessLayer;
using System.Linq;

namespace Domain.Tests.Services
{
    [TestClass]
    public class GroupServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IRepository<Expense, int>> _expensesRepositoryMock;
        private Mock<IRepository<Group, int>> _groupsRepositoryMock;
        private Mock<IRepository<Transfer, int>> _transfersRepositoryMock;
        private Mock<IRepository<User, string>> _usersRepositoryMock;
        GroupService _groupService;

        [TestInitialize]
        public void Setup()
        {
            _expensesRepositoryMock = new Mock<IRepository<Expense, int>>();
            _groupsRepositoryMock = new Mock<IRepository<Group, int>>();
            _transfersRepositoryMock = new Mock<IRepository<Transfer, int>>();
            _usersRepositoryMock = new Mock<IRepository<User, string>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.SetupGet(p => p.ExpensesRepository).Returns(_expensesRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(p => p.GroupsRepository).Returns(_groupsRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(p => p.TransfersRepository).Returns(_transfersRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(p => p.UsersRepository).Returns(_usersRepositoryMock.Object);
        }

        [TestMethod]
        public void GetAllGroups()
        {
            // Arrange
            var userId = "user";
            var groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "group1" },
                new Group { GroupId = 2, Name = "group2" }
            };
            var user = new User
            {
                Id = userId,
                Groups = groups
            };
            _usersRepositoryMock.Setup(m => m.Get(userId)).Returns(user);
            _groupService = new GroupService(_unitOfWorkMock.Object);

            // Act
            var result = _groupService.GetAllGroups(userId);

            // Assert
            _usersRepositoryMock.Verify(x => x.Get(userId), Times.Once());
            Assert.AreEqual(groups, result);
        }

        [TestMethod]
        public void CreateGroup_GroupExists()
        {
            // Arrange
            var group = "group";
            var secret = "secret";
            var user = "user";
            var groups = new List<Group>
            {
                new Group { Name = group },
                new Group { Name = "other group" }
            }.AsQueryable();
            _groupsRepositoryMock.Setup(g => g.GetAll()).Returns(groups);
            _groupService = new GroupService(_unitOfWorkMock.Object);

            // Act
            var result = _groupService.CreateGroup(group, secret, user);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateGroup()
        {
            // Arrange
            var group = "group";
            var secret = "secret";
            var user = "user";
            _usersRepositoryMock.Setup(repo => repo.Get(user)).Returns(new User { Id = user });
            _groupService = new GroupService(_unitOfWorkMock.Object);

            // Act
            var result = _groupService.CreateGroup(group, secret, user);

            // Assert
            Assert.IsTrue(result);
            _groupsRepositoryMock.Verify(repo => repo.Add(It.Is<Group>(
                g => g.Name == group
                && g.Secret == secret
                && g.Members.Any(u => u.Id == user))),
                Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(GroupNotFoundException))]
        public void JoinGroup_GroupDoesNotExists()
        {
            // Arrange
            var group = "group";
            var secret = "secret";
            var user = "user";
            _groupsRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Group> { new Group { Name = "other group" } }.AsQueryable());
            _groupService = new GroupService(_unitOfWorkMock.Object);

            // Act
            _groupService.JoinGroup(group, secret, user);
        }

        [TestMethod]
        [ExpectedException(typeof(WrongGroupPasswordException))]
        public void JoinGroup_GroupHasDifferentPassword()
        {
            // Arrange
            var group = "group";
            var secret = "secret";
            var user = "user";
            _groupsRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Group> { new Group { Name = group, Secret = "other secret" } }.AsQueryable());
            _groupService = new GroupService(_unitOfWorkMock.Object);

            // Act
            _groupService.JoinGroup(group, secret, user);
        }

        [TestMethod]
        public void JoinGroup()
        {
            // Arrange
            var group = "group";
            var secret = "secret";
            var user = "user";
            var groupObject = new Group { Name = group, Secret = secret };
            _groupsRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Group> { groupObject }.AsQueryable());
            _usersRepositoryMock.Setup(repo => repo.Get(user)).Returns(new User { Id = user });
            _groupService = new GroupService(_unitOfWorkMock.Object);

            // Act
            _groupService.JoinGroup(group, secret, user);

            // Assert
            _groupsRepositoryMock.Verify(x => x.Update(It.Is<Group>(
                g => g.Name == group 
                && g.Secret == secret 
                && g.Members.Any(u => u.Id == user))),
                Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void EditGroup_NewNameHasTakenGroupName()
        {
            // Arrange
            var newName = "notSoNew";
            _groupsRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Group> { new Group { Name = newName } }.AsQueryable());
            _groupService = new GroupService(_unitOfWorkMock.Object);

            // Act
            var result = _groupService.EditGroup(1, newName);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EditGroup()
        {
            // Arrange
            var id = 1;
            var newName = "newName";
            var originalGroup = new Group { GroupId = id, Name = "oldName" };
            _groupsRepositoryMock.Setup(repo => repo.Get(id)).Returns(originalGroup);
            _groupService = new GroupService(_unitOfWorkMock.Object);

            // Act
            var result = _groupService.EditGroup(id, newName);

            // Assert
            Assert.IsTrue(result);
            _groupsRepositoryMock.Verify(x => x.Update(It.Is<Group>(g => g.Name == newName)), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void RemoveGroup_WrongPassword()
        {
            // Arrange
            var groupId = 1;
            var secret = "secret";
            _groupsRepositoryMock.Setup(x => x.Get(groupId)).Returns(new Group { GroupId = groupId, Secret = "terces" });
            _groupService = new GroupService(_unitOfWorkMock.Object);

            // Act
            var result = _groupService.RemoveGroup(groupId, secret);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveGroup()
        {
            // Arrange
            var groupId = 1;
            var secret = "secret";
            var group = new Group { GroupId = groupId, Secret = secret };
            _groupsRepositoryMock.Setup(x => x.Get(groupId)).Returns(group);
            _groupService = new GroupService(_unitOfWorkMock.Object);

            // Act
            var result = _groupService.RemoveGroup(groupId, secret);

            // Assert
            Assert.IsTrue(result);
            _groupsRepositoryMock.Verify(x => x.Remove(group), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChanges(), Times.Once());
        }
    }
}