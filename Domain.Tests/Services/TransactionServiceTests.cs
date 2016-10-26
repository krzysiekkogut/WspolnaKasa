using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataAccessLayer;
using Domain.Services;
using Domain.Entities;
using Domain.Models;
using Domain.Exceptions;

namespace Domain.Tests.Services
{
    [TestClass]
    public class TransactionServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IRepository<Expense, int>> _expensesRepositoryMock;
        private Mock<IRepository<Group, int>> _groupsRepositoryMock;
        private Mock<IRepository<Transfer, int>> _transfersRepositoryMock;
        private Mock<IRepository<User, string>> _usersRepositoryMock;
        private TransactionService _transactionService;

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
            _transactionService = new TransactionService(_unitOfWorkMock.Object);
        }

        [TestMethod]
        public void GetAllExpenses()
        {
            // Arrange
            var userId = "testUser";
            var expensesPaid = new List<Expense>
            {
                new Expense { ExpenseId = 1 },
                new Expense { ExpenseId = 3 }
            };
            var expensesParticipated = new List<Expense>
            {
                new Expense { ExpenseId = 2 },
                new Expense { ExpenseId = 4 }
            };
            var user = new User
            {
                Id = userId,
                ExpensesPaid = expensesPaid,
                ExpensesParticipated = expensesParticipated
            };
            _usersRepositoryMock.Setup(repo => repo.Get(userId)).Returns(user);

            // Act
            var result = _transactionService.GetAllExpenses(userId);

            // Assert
            CollectionAssert.AreEquivalent(expensesPaid.Union(expensesParticipated).ToList(), result.ToList());
            AssertSortedDescending(result.Select(e => e.Date).ToArray());
        }

        [TestMethod]
        public void GetAllExpenses_WithGroupId()
        {
            // Arrange
            var userId = "testUser";
            var groupId = 1;
            var otherGroupId = 2;
            var expensesPaid = new List<Expense>
            {
                new Expense { ExpenseId = 1, GroupId = otherGroupId },
                new Expense { ExpenseId = 3, GroupId = groupId }
            };
            var expensesParticipated = new List<Expense>
            {
                new Expense { ExpenseId = 2, GroupId = groupId },
                new Expense { ExpenseId = 4, GroupId = otherGroupId }
            };
            var expectedExpenses = expensesPaid.Union(expensesParticipated).Where(e => e.GroupId == groupId).ToList();
            var user = new User
            {
                Id = userId,
                ExpensesPaid = expensesPaid,
                ExpensesParticipated = expensesParticipated
            };
            _usersRepositoryMock.Setup(repo => repo.Get(userId)).Returns(user);

            // Act
            var result = _transactionService.GetAllExpenses(userId, groupId);

            // Assert
            CollectionAssert.AreEquivalent(expectedExpenses, result.ToList());
            AssertSortedDescending(result.Select(e => e.Date).ToArray());
        }

        [TestMethod]
        public void GetAllSentAndReceivedTransfers()
        {

            // Arrange
            var userId = "testUser";
            var transfersReceived = new List<Transfer>
            {
                new Transfer { TransferId = 1, Date = new DateTime(2016, 10, 24) },
                new Transfer { TransferId = 3, Date = new DateTime(2016, 10, 22) }

            };
            var transfersSent = new List<Transfer>
            {
                new Transfer { TransferId = 2, Date = new DateTime(2016, 10, 23) },
                new Transfer { TransferId = 4, Date = new DateTime(2016, 10, 28) }
            };
            var user = new User
            {
                Id = userId,
                TransfersReceived = transfersReceived,
                TransfersSent = transfersSent
            };
            var expectedTransfers = transfersReceived.Union(transfersSent);
            _usersRepositoryMock.Setup(repo => repo.Get(userId)).Returns(user);

            // Act
            var result = _transactionService.GetAllSentAndReceivedTransfers(userId);

            // Assert
            CollectionAssert.AreEquivalent(expectedTransfers.ToList(), result.ToList());
            AssertSortedDescending(result.Select(t => t.Date).ToArray());
        }

        [TestMethod]
        public void GetAllSentAndReceivedTransfers_WithGroupId()
        {

            // Arrange
            var userId = "testUser";
            var groupId = 1;
            var otherGroupId = 2;
            var transfersReceived = new List<Transfer>
            {
                new Transfer { TransferId = 1, Date = new DateTime(2016, 10, 24), GroupId = groupId },
                new Transfer { TransferId = 3, Date = new DateTime(2016, 10, 22), GroupId = otherGroupId }

            };
            var transfersSent = new List<Transfer>
            {
                new Transfer { TransferId = 2, Date = new DateTime(2016, 10, 23), GroupId = otherGroupId },
                new Transfer { TransferId = 4, Date = new DateTime(2016, 10, 28), GroupId = groupId }
            };
            var user = new User
            {
                Id = userId,
                TransfersReceived = transfersReceived,
                TransfersSent = transfersSent
            };
            var expectedTransfers = transfersReceived.Union(transfersSent).Where(t => t.GroupId == groupId);
            _usersRepositoryMock.Setup(repo => repo.Get(userId)).Returns(user);

            // Act
            var result = _transactionService.GetAllSentAndReceivedTransfers(userId, groupId);

            // Assert
            CollectionAssert.AreEquivalent(expectedTransfers.ToList(), result.ToList());
            AssertSortedDescending(result.Select(t => t.Date).ToArray());
        }

        [TestMethod]
        public void GetSummaryForUser_NoExpenses_NoTransfers_EmptySettlements()
        {
            // Arrange
            var userId = "currentUser";
            var user = new User
            {
                Id = userId,
                TransfersSent = new List<Transfer>(),
                TransfersReceived = new List<Transfer>(),
                ExpensesPaid = new List<Expense>(),
                ExpensesParticipated = new List<Expense>()
            };
            _usersRepositoryMock.Setup(repo => repo.Get(userId)).Returns(user);

            // Act
            var result = _transactionService.GetSummaryForUser(userId);

            // Assert
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetSummaryForUser_NoExpenses_OnlyOneSentTransferToOneUser()
        {
            // Arrange
            var currentUserId = "currentUser";
            var otherUserId = "otherUser";
            var otherUser = new User
            {
                Id = otherUserId,
                DisplayName = "Other user"
            };
            var sentTransfers = new List<Transfer>
            {
                new Transfer
                {
                    Amount = 100,
                    ReceiverId = otherUserId,
                    Receiver = otherUser,
                    SenderId = currentUserId
                }
            };
            var currentUser = new User
            {
                Id = currentUserId,
                TransfersSent = sentTransfers,
                TransfersReceived = new List<Transfer>(),
                ExpensesPaid = new List<Expense>(),
                ExpensesParticipated = new List<Expense>()
            };
            _usersRepositoryMock.Setup(repo => repo.Get(currentUserId)).Returns(currentUser);
            _usersRepositoryMock.Setup(repo => repo.Get(otherUserId)).Returns(otherUser);

            // Act
            var result = _transactionService.GetSummaryForUser(currentUserId);

            // Assert
            Assert.AreEqual(new Settlement { Amount = 100, User = otherUser }, result.Single());
        }

        [TestMethod]
        public void GetSummaryForUser_NoExpenses_OnlySentTwoTransfersToOneUser()
        {
            // Arrange
            var currentUserId = "currentUser";
            var otherUserId = "otherUser";
            var otherUser = new User
            {
                Id = otherUserId,
                DisplayName = "Other user"
            };
            var sentTransfers = new List<Transfer>
            {
                new Transfer { Amount = 50, ReceiverId = otherUserId, Receiver = otherUser, SenderId = currentUserId },
                new Transfer { Amount = 50, ReceiverId = otherUserId, Receiver = otherUser, SenderId = currentUserId }
            };
            var currentUser = new User
            {
                Id = currentUserId,
                TransfersSent = sentTransfers,
                TransfersReceived = new List<Transfer>(),
                ExpensesPaid = new List<Expense>(),
                ExpensesParticipated = new List<Expense>()
            };
            _usersRepositoryMock.Setup(repo => repo.Get(currentUserId)).Returns(currentUser);
            _usersRepositoryMock.Setup(repo => repo.Get(otherUserId)).Returns(otherUser);

            // Act
            var result = _transactionService.GetSummaryForUser(currentUserId);

            // Assert
            Assert.AreEqual(new Settlement { Amount = 100, User = otherUser }, result.Single());
        }

        [TestMethod]
        public void GetSummaryForUser_NoExpenses_OnlySentThreeTransfersToTwoUsers()
        {
            // Arrange
            var currentUserId = "currentUser";
            var otherUser1Id = "otherUser1";
            var otherUser2Id = "otherUser2";
            var otherUser1 = new User
            {
                Id = otherUser1Id,
                DisplayName = "Other user 1"
            };
            var otherUser2 = new User
            {
                Id = otherUser2Id,
                DisplayName = "Other user 2"
            };
            var currentUser = new User
            {
                Id = currentUserId,
                TransfersSent = new List<Transfer>
                {
                    new Transfer
                    {
                        Amount = 50,
                        ReceiverId = otherUser1Id,
                        Receiver = otherUser1,
                        SenderId = currentUserId
                    },
                    new Transfer
                    {
                        Amount = 50,
                        ReceiverId = otherUser1Id,
                        Receiver = otherUser1,
                        SenderId = currentUserId
                    },
                    new Transfer
                    {
                        Amount = 66,
                        ReceiverId = otherUser2Id,
                        Receiver = otherUser2,
                        SenderId = currentUserId
                    }
                },
                TransfersReceived = new List<Transfer>(),
                ExpensesPaid = new List<Expense>(),
                ExpensesParticipated = new List<Expense>()
            };
            _usersRepositoryMock.Setup(repo => repo.Get(currentUserId)).Returns(currentUser);

            // Act
            var result = _transactionService.GetSummaryForUser(currentUserId);

            // Assert
            CollectionAssert.AreEqual(
                new List<Settlement>
                {
                    new Settlement { Amount = 100, User = otherUser1 },
                    new Settlement { Amount = 66, User = otherUser2 }
                },
                result.ToList());
        }

        [TestMethod]
        public void GetSummaryForUser_NoExpenses_SentThreeTransfersToTwoUsersOneReceived()
        {
            // Arrange
            var currentUserId = "currentUser";
            var otherUser1Id = "otherUser1";
            var otherUser2Id = "otherUser2";
            User currentUser = null;
            var otherUser1 = new User
            {
                Id = otherUser1Id,
                DisplayName = "Other User 1"
            };
            var otherUser2 = new User
            {
                Id = otherUser2Id,
                DisplayName = "Other User 2"
            };
            currentUser = new User
            {
                Id = currentUserId,
                DisplayName = "Current User",
                ExpensesPaid = new List<Expense>(),
                ExpensesParticipated = new List<Expense>(),
                TransfersSent = new List<Transfer>
                {
                    new Transfer { Amount = 50, ReceiverId = otherUser1Id, Receiver = otherUser1, SenderId = currentUserId },
                    new Transfer { Amount = 50, ReceiverId = otherUser1Id, Receiver = otherUser1, SenderId = currentUserId },
                    new Transfer { Amount = 66, ReceiverId = otherUser2Id, Receiver = otherUser2, SenderId = currentUserId }
                },
                TransfersReceived = new List<Transfer>
                {
                    new Transfer { Amount = 40, ReceiverId = currentUserId, Receiver = currentUser, SenderId = otherUser2Id, Sender = otherUser2 }
                }
            };
            _usersRepositoryMock.Setup(repo => repo.Get(currentUserId)).Returns(currentUser);

            // Act
            var result = _transactionService.GetSummaryForUser(currentUserId);

            // Assert
            CollectionAssert.AreEqual(
                new List<Settlement>
                {
                    new Settlement { Amount = 100, User = otherUser1 },
                    new Settlement { Amount = 26, User = otherUser2 }
                },
                result.ToList());
        }

        [TestMethod]
        public void GetSummaryForUser_PaidExpenses_SentThreeTransfersToTwoUsersOneReceived()
        {
            // Arrange
            var currentUserId = "currentUser";
            var otherUser1Id = "otherUser1";
            var otherUser2Id = "otherUser2";
            var otherUser1 = new User { Id = otherUser1Id };
            var otherUser2 = new User { Id = otherUser2Id };
            var currentUser = new User();
            currentUser.Id = currentUserId;
            currentUser.TransfersReceived = new List<Transfer>
            {
                new Transfer { Amount = 40, ReceiverId = currentUserId, SenderId = otherUser2Id, Sender = otherUser2 }
            };
            currentUser.TransfersSent = new List<Transfer>
            {
                new Transfer { Amount = 50, ReceiverId = otherUser1Id, Receiver = otherUser1, SenderId = currentUserId },
                new Transfer { Amount = 50, ReceiverId = otherUser1Id, Receiver = otherUser1, SenderId = currentUserId },
                new Transfer { Amount = 66, ReceiverId = otherUser2Id, Receiver = otherUser2, SenderId = currentUserId }
            };
            currentUser.ExpensesPaid = new List<Expense>
            {
                new Expense
                    {
                        Amount = 30, UserPayingId = currentUserId, Participants = new List<User> { currentUser, otherUser1, otherUser2 }
                    }
            };
            currentUser.ExpensesParticipated = new List<Expense>();
            var expectedResult = new List<Settlement>
                {
                    new Settlement { Amount = 110, User = otherUser1 },
                    new Settlement { Amount = 36, User = otherUser2 }
                };
            _usersRepositoryMock.Setup(repo => repo.Get(currentUserId)).Returns(currentUser);

            // Act
            var result = _transactionService.GetSummaryForUser(currentUserId);

            // Assert
            CollectionAssert.AreEqual(expectedResult, result.ToList());
        }

        [TestMethod]
        public void GetSummaryForUser_PaidExpensesAndReceived_SentThreeTransfersToTwoUsersOneReceived()
        {
            // Arrange
            var currentUserId = "currentUser";
            var otherUser1Id = "otherUser1";
            var otherUser2Id = "otherUser2";
            var otherUser1 = new User { Id = otherUser1Id };
            var otherUser2 = new User { Id = otherUser2Id };
            var currentUser = new User();
            currentUser.Id = currentUserId;
            currentUser.TransfersSent = new List<Transfer>
            {
                new Transfer { Amount = 50, ReceiverId = otherUser1Id, Receiver = otherUser1, SenderId = currentUserId },
                new Transfer { Amount = 50, ReceiverId = otherUser1Id, Receiver = otherUser1, SenderId = currentUserId },
                new Transfer { Amount = 66, ReceiverId = otherUser2Id, Receiver = otherUser2, SenderId = currentUserId }
            };
            currentUser.TransfersReceived = new List<Transfer>
            {
                new Transfer { Amount = 40, ReceiverId = currentUserId, SenderId = otherUser2Id, Sender = otherUser2 }
            };
            currentUser.ExpensesPaid = new List<Expense>
            {
                new Expense
                {
                    Amount = 30, UserPayingId = currentUserId, Participants = new List<User> { currentUser, otherUser1, otherUser2 }
                }
            };
            currentUser.ExpensesParticipated = new List<Expense>
            {
                new Expense
                {
                    Amount = 75, UserPayingId = otherUser1Id, UserPaying = otherUser1, Participants = new List<User> { currentUser, otherUser1, otherUser2 }
                }
            };
            var expectedResult = new List<Settlement>
            {
                new Settlement { Amount = 85, User = otherUser1 },
                new Settlement { Amount = 36, User = otherUser2 }
            };
            _usersRepositoryMock.Setup(repo => repo.Get(currentUserId)).Returns(currentUser);

            // Act
            var result = _transactionService.GetSummaryForUser(currentUserId);

            // Assert
            CollectionAssert.AreEqual(expectedResult, result.ToList());
        }

        [TestMethod]
        public void GetSummaryForUser_PaidExpensesAndReceived_SentThreeTransfersToTwoUsersOneReceived_TwoGroups_OneFiltered()
        {
            var currentUserId = "currentUser";
            var otherUser1Id = "otherUser1";
            var otherUser2Id = "otherUser2";
            var filteredGroup = 1;
            var otherGroup = 2;
            var otherUser1 = new User { Id = otherUser1Id };
            var otherUser2 = new User { Id = otherUser2Id };
            var currentUser = new User();
            currentUser.Id = currentUserId;
            currentUser.TransfersSent = new List<Transfer>
            {
                new Transfer { Amount = 50, ReceiverId = otherUser1Id, Receiver = otherUser1, SenderId = currentUserId, GroupId = filteredGroup },
                new Transfer { Amount = 66, ReceiverId = otherUser2Id, Receiver = otherUser2, SenderId = currentUserId, GroupId = filteredGroup },
                new Transfer { Amount = 66, ReceiverId = otherUser2Id, Receiver = otherUser2, SenderId = currentUserId, GroupId = otherGroup }
            };
            currentUser.TransfersReceived = new List<Transfer>
            {
                new Transfer { Amount = 40, ReceiverId = currentUserId, SenderId = otherUser2Id, Sender = otherUser2, GroupId = otherGroup }
            };
            currentUser.ExpensesPaid = new List<Expense>
            {
                new Expense
                {
                    Amount = 30, UserPayingId = currentUserId, GroupId = filteredGroup,
                    Participants = new List<User> { currentUser, otherUser1, otherUser2 }
                }
            };
            currentUser.ExpensesParticipated = new List<Expense>
            {
                new Expense
                {
                    Amount = 75, UserPayingId = otherUser1Id, UserPaying = otherUser1, GroupId = otherGroup,
                    Participants = new List<User> { currentUser, otherUser1, otherUser2 }
                }
            };
            var expectedResult = new List<Settlement>
            {
                new Settlement { Amount = 60, User = otherUser1 },
                new Settlement { Amount = 76, User = otherUser2 }
            };
            _usersRepositoryMock.Setup(repo => repo.Get(currentUserId)).Returns(currentUser);

            // Act
            var result = _transactionService.GetSummaryForUser(currentUserId, filteredGroup);

            // Assert
            CollectionAssert.AreEqual(expectedResult, result.ToList());
        }

        [TestMethod]
        public void AddExpense()
        {
            // Arrange
            var groupId = 1;
            var userId = "user";
            var otherUserId = "otherUser";
            var group = new Group
            {
                GroupId = groupId,
                Members = new List<User> { new User { Id = userId }, new User { Id = otherUserId } }
            };
            var description = "desc";
            var date = new DateTime(2015, 1, 1);
            var amount = 15.11;
            var participants = new List<string> { userId, otherUserId };
            var expectedExpense = new Expense
            {
                Amount = amount,
                Date = date,
                Description = description,
                GroupId = groupId,
                UserPayingId = userId
            };
            _groupsRepositoryMock.Setup(x => x.Get(groupId)).Returns(group);

            // Act
            _transactionService.AddExpense(userId, groupId, description, date, amount, participants);

            // Assert
            _expensesRepositoryMock.Verify(x => x.Add(expectedExpense), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ExpenseParticipantsMustBeGroupMembersExecption))]
        public void AddExpense_ShouldNotBeAbleToAddParticipantsIfNotGroupMembers()
        {
            // Arrange
            var groupId = 1;
            var userId = "user";
            var otherUser1 = new User { Id = "1" };
            var group = new Group
            {
                GroupId = groupId,
                Members = new List<User> { new User { Id = userId }, otherUser1 }
            };
            var description = "desc";
            var date = new DateTime(2015, 1, 1);
            var amount = 15.11;
            var participants = new List<string> { otherUser1.Id, "2" };
            var expectedExpense = new Expense
            {
                Amount = amount,
                Date = date,
                Description = description,
                GroupId = groupId,
                UserPayingId = userId
            };
            _groupsRepositoryMock.Setup(x => x.Get(groupId)).Returns(group);

            // Act
            _transactionService.AddExpense(userId, groupId, description, date, amount, participants);
        }


        [TestMethod]
        public void EditExpense_TheSameGroup()
        {
            // Arrange
            var expenseId = 1;
            var userId = "user";
            var groupId = 1;
            var description = "desc";
            var date = new DateTime(2015, 1, 1);
            var amount = 15.11;
            var participant1Id = "1";
            var participant2Id = "2";
            var participants = new List<string> { userId, participant1Id, participant2Id };
            var expenseOriginal = new Expense
            {
                Amount = 5,
                Date = new DateTime(2013, 1, 1),
                Description = "old",
                GroupId = groupId,
                UserPayingId = userId,
                ExpenseId = expenseId
            };
            var expectedExpense = new Expense
            {
                Amount = amount,
                Date = date,
                Description = description,
                GroupId = groupId,
                UserPayingId = userId,
                ExpenseId = expenseId
            };
            _expensesRepositoryMock.Setup(x => x.Get(expenseId)).Returns(expenseOriginal);
            _usersRepositoryMock.Setup(x => x.Get(userId)).Returns(new User { Id = userId });
            _usersRepositoryMock.Setup(x => x.Get(participant1Id)).Returns(new User { Id = participant1Id });
            _usersRepositoryMock.Setup(x => x.Get(participant2Id)).Returns(new User { Id = participant2Id });

            // Act
            _transactionService.EditExpense(userId, expenseId, groupId, description, date, amount, participants);

            // Assert
            _expensesRepositoryMock.Verify(x => x.Update(expectedExpense), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ExpenseParticipantsMustBeGroupMembersExecption))]
        public void EditExpense_GroupChanged_ShouldNotBeAbleToAddParticipantsIfNotGroupMembers()
        {
            // Arrange
            var expenseId = 1;
            var userId = "user";
            var groupId = 1;
            var description = "desc";
            var date = new DateTime(2015, 1, 1);
            var amount = 15.11;
            var participant1Id = "1";
            var participant2Id = "2";
            var participants = new List<string> { participant1Id, participant2Id };
            var currentUser = new User { Id = userId };
            var otherUser1 = new User { Id = participant1Id };
            var otherUser2 = new User { Id = participant2Id };
            var expenseOriginal = new Expense
            {
                Amount = 5,
                Date = new DateTime(2013, 1, 1),
                Description = "old",
                GroupId = 2,
                UserPayingId = userId,
                ExpenseId = expenseId
            };
            var expectedExpense = new Expense
            {
                Amount = amount,
                Date = date,
                Description = description,
                GroupId = groupId,
                UserPayingId = userId,
                ExpenseId = expenseId
            };
            var newGroup = new Group
            {
                GroupId = groupId,
                Members = new List<User> { currentUser, otherUser1 }
            };
            var oldGroup = new Group
            {
                GroupId = groupId,
                Members = new List<User> { otherUser1, otherUser2 }
            };
            _expensesRepositoryMock.Setup(x => x.Get(expenseId)).Returns(expenseOriginal);
            _groupsRepositoryMock.Setup(x => x.Get(groupId)).Returns(new Group { GroupId = groupId });
            _usersRepositoryMock.Setup(x => x.Get(participant1Id)).Returns(otherUser1);
            _usersRepositoryMock.Setup(x => x.Get(participant2Id)).Returns(otherUser2);

            // Act
            _transactionService.EditExpense(userId, expenseId, groupId, description, date, amount, participants);
        }

        [TestMethod]
        [ExpectedException(typeof(CannotEditOtherUsersExpensesException))]
        public void EditExpense_OtherUserRequest()
        {
            // Arrange
            var expenseId = 1;
            var userId = "user";
            var groupId = 1;
            var description = "desc";
            var date = new DateTime(2015, 1, 1);
            var amount = 15.11;
            var participant1Id = "1";
            var participant2Id = "2";
            var participants = new List<string> { participant1Id, participant2Id };
            var expenseOriginal = new Expense
            {
                Amount = 5,
                Date = new DateTime(2013, 1, 1),
                Description = "old",
                GroupId = groupId,
                UserPayingId = "otherUser",
                ExpenseId = expenseId
            };
            _usersRepositoryMock.Setup(x => x.Get(participant1Id)).Returns(new User { Id = participant1Id });
            _usersRepositoryMock.Setup(x => x.Get(participant1Id)).Returns(new User { Id = participant1Id });
            _expensesRepositoryMock.Setup(x => x.Get(expenseId)).Returns(expenseOriginal);

            // Act
            _transactionService.EditExpense(userId, expenseId, groupId, description, date, amount, participants);
        }

        [TestMethod]
        public void RemoveExpense()
        {
            // Arrange
            var expenseId = 1;
            var userId = "user";
            var expense = new Expense { ExpenseId = expenseId, UserPayingId = userId };
            _expensesRepositoryMock.Setup(x => x.Get(expenseId)).Returns(expense);

            // Act 
            _transactionService.RemoveExpense(userId, expenseId);

            // Assert
            _expensesRepositoryMock.Verify(x => x.Remove(expense), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(CannotEditOtherUsersExpensesException))]
        public void RemoveExpense_OtherUserRequest()
        {
            // Arrange
            var expenseId = 1;
            var expense = new Expense { ExpenseId = expenseId, UserPayingId = "otherUser" };
            _expensesRepositoryMock.Setup(x => x.Get(expenseId)).Returns(expense);

            // Act
            _transactionService.RemoveExpense("user", expenseId);
        }

        [TestMethod]
        public void RemoveTransfer()
        {
            // Arrange
            var transferId = 1;
            var userId = "user";
            var transfer = new Transfer { TransferId = transferId, SenderId = userId };
            _transfersRepositoryMock.Setup(x => x.Get(transferId)).Returns(transfer);

            // Act
            _transactionService.RemoveTransfer(userId, transferId);

            // Assert
            _transfersRepositoryMock.Verify(x => x.Remove(transfer), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(CannotEditOtherUsersTransfersException))]
        public void RemoveTransfer_OtherUserRequest()
        {
            // Arrange
            var transferId = 1;
            var transfer = new Transfer { TransferId = transferId, SenderId = "otherUser" };
            _transfersRepositoryMock.Setup(x => x.Get(transferId)).Returns(transfer);

            // Act
            _transactionService.RemoveTransfer("user", transferId);
        }

        private static void AssertSortedDescending(DateTime[] dates)
        {
            for (int i = 1; i < dates.Length; i++)
            {
                Assert.IsTrue(dates[i] <= dates[i - 1]);
            }
        }
    }
}