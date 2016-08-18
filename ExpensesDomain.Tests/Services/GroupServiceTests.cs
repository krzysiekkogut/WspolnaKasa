using System.Collections.Generic;
using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Entities.ExpensesDomain;
using ExpensesDomain.Exceptions;
using ExpensesDomain.Repositories;
using ExpensesDomain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ExpensesDomain.Tests.Services
{
    [TestClass]
    public class GroupServiceTests
    {
        Mock<IGroupRepository> groupRepositoryMock = new Mock<IGroupRepository>();
        Mock<IApplicationUserRepository> applicationUserRepositoryMock = new Mock<IApplicationUserRepository>();
        GroupService groupService;

        [TestMethod]
        public void GetAllGroups()
        {
            var userId = "krzysiek";
            groupService = new GroupService(groupRepositoryMock.Object, applicationUserRepositoryMock.Object);

            groupService.GetAllGroups(userId);

            groupRepositoryMock.Verify(x => x.GetAll(userId), Times.Once());
        }

        [TestMethod]
        public void CreateGroup_GroupExists()
        {
            var group = "grupa";
            var secret = "haslo";
            var user = "user";
            groupRepositoryMock.Setup(x => x.Exists(group)).Returns(true);
            groupService = new GroupService(groupRepositoryMock.Object, applicationUserRepositoryMock.Object);

            var result = groupService.CreateGroup(group, secret, user);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateGroup()
        {
            var group = "grupa";
            var secret = "haslo";
            var user = "user";
            groupRepositoryMock.Setup(x => x.Exists(group)).Returns(false);
            groupService = new GroupService(groupRepositoryMock.Object, applicationUserRepositoryMock.Object);

            var result = groupService.CreateGroup(group, secret, user);

            Assert.IsTrue(result);
            groupRepositoryMock.Verify(x => x.Add(new Group { Name = group, Secret = secret }), Times.Once());
            groupRepositoryMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(GroupNotFoundException))]
        public void JoinGroup_GroupDoesNotExists()
        {
            var group = "grupa";
            var secret = "haslo";
            var user = "user";
            groupRepositoryMock.Setup(x => x.Exists(group)).Returns(false);
            groupService = new GroupService(groupRepositoryMock.Object, applicationUserRepositoryMock.Object);

            groupService.JoinGroup(group, secret, user);
        }

        [TestMethod]
        [ExpectedException(typeof(WrongGroupPasswordException))]
        public void JoinGroup_GroupHasDifferentPassword()
        {
            var group = "grupa";
            var secret = "haslo";
            var user = "user";
            groupRepositoryMock.Setup(x => x.Exists(group)).Returns(true);
            groupRepositoryMock.Setup(x => x.Get(group)).Returns(new Group { Name = group, Secret = "olsah" });
            groupService = new GroupService(groupRepositoryMock.Object, applicationUserRepositoryMock.Object);

            groupService.JoinGroup(group, secret, user);
        }

        [TestMethod]
        public void JoinGroup()
        {
            var group = "grupa";
            var secret = "haslo";
            var user = "user";
            var groupObject = new Group { Name = group, Secret = secret, Members = new List<ApplicationUser>() };
            groupRepositoryMock.Setup(x => x.Exists(group)).Returns(true);
            groupRepositoryMock.Setup(x => x.Get(group)).Returns(groupObject);
            applicationUserRepositoryMock.Setup(x => x.GetUser(user)).Returns(new ApplicationUser { Id = user });
            groupService = new GroupService(groupRepositoryMock.Object, applicationUserRepositoryMock.Object);

            groupService.JoinGroup(group, secret, user);

            groupRepositoryMock.Verify(x => x.Update(groupObject), Times.Once());
            groupRepositoryMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void EditGroup_NewNameHasTakenGroupName()
        {
            var newName = "notSoNew";
            groupRepositoryMock.Setup(x => x.Exists(newName)).Returns(true);
            groupService = new GroupService(groupRepositoryMock.Object, applicationUserRepositoryMock.Object);

            var result = groupService.EditGroup(1, newName);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EditGroup()
        {
            var id = 1;
            var newName = "newName";
            var original = new Group { GroupId = id, Name = "oldName" };
            var updated = new Group { GroupId = id, Name = newName };
            groupRepositoryMock.Setup(x => x.Exists(newName)).Returns(false);
            groupRepositoryMock.Setup(x => x.Get(id)).Returns(original);
            groupService = new GroupService(groupRepositoryMock.Object, applicationUserRepositoryMock.Object);

            var result = groupService.EditGroup(id, newName);

            Assert.IsTrue(result);
            groupRepositoryMock.Verify(x => x.Update(updated), Times.Once());
            groupRepositoryMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void RemoveGroup_WrongPassword()
        {
            var groupId = 1;
            var secret = "secret";
            groupRepositoryMock.Setup(x => x.Get(groupId)).Returns(new Group { GroupId = groupId, Secret = "terces" });
            groupService = new GroupService(groupRepositoryMock.Object, applicationUserRepositoryMock.Object);

            var result = groupService.RemoveGroup(groupId, secret);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveGroup()
        {
            var groupId = 1;
            var secret = "secret";
            var group = new Group { GroupId = groupId, Secret = secret };
            groupRepositoryMock.Setup(x => x.Get(groupId)).Returns(group);
            groupService = new GroupService(groupRepositoryMock.Object, applicationUserRepositoryMock.Object);

            var result = groupService.RemoveGroup(groupId, secret);

            Assert.IsTrue(result);
            groupRepositoryMock.Verify(x => x.Remove(group), Times.Once());
            groupRepositoryMock.Verify(x => x.SaveChanges(), Times.Once());
        }
    }
}