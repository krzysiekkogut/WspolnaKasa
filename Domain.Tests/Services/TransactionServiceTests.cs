using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataAccessLayer.Repositories;
using Domain.Services;
using Domain.Entities;
using Domain.Models;

namespace ExpensesDomain.Tests.Services
{
    [TestClass]
    public class TransactionServiceTests
    {
        private TransactionService transactionService;
        private Mock<IExpensesRepository> expensesRepository;
        private Mock<ITransferRepository> transferRepository;
        private Mock<IGroupService> groupService;
        private Mock<IUserRepository> userRepository;


        [TestInitialize]
        public void Setup()
        {
            expensesRepository = new Mock<IExpensesRepository>();
            transferRepository = new Mock<ITransferRepository>();
            groupService = new Mock<IGroupService>();
            userRepository = new Mock<IUserRepository>();
            transactionService = new TransactionService(expensesRepository.Object, transferRepository.Object, groupService.Object, userRepository.Object);
        }

        [TestMethod]
        public void GetAllExpenses()
        {
            var userId = "testUser";
            transactionService.GetAllExpenses(userId);

            expensesRepository.Verify(x => x.GetAll(userId), Times.Once());
        }

        [TestMethod]
        public void GetAllExpenses_WithGroupId()
        {
            var userId = "testUser";
            var groupId = 1;

            transactionService.GetAllExpenses(userId, groupId);

            expensesRepository.Verify(x => x.GetAll(userId, groupId), Times.Once());
        }

        [TestMethod]
        public void GetAllSentAndReceivedTransfers()
        {
            var userId = "testUser";

            transactionService.GetAllSentAndReceivedTransfers(userId);

            transferRepository.Verify(x => x.GetAllSentAndReceived(userId), Times.Once());
        }

        [TestMethod]
        public void GetAllSentAndReceivedTransfers_WithGroupId()
        {
            var userId = "testUser";
            var groupId = 1;

            transactionService.GetAllSentAndReceivedTransfers(userId, groupId);

            transferRepository.Verify(x => x.GetAllSentAndReceived(userId, groupId), Times.Once());
        }

        [TestMethod]
        public void GetSummaryForUser_NoExpenses_NoTransfers_EmptySettlements()
        {
            var user = "currentUser";

            var result = transactionService.GetSummaryForUser(user);

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetSummaryForUser_NoExpenses_OnlySentTransfersToOneUser()
        {
            var currentUser = "currentUser";
            var otherUser = "otherUser";

            transferRepository
                .Setup(x => x.GetAllSentAndReceived(currentUser))
                .Returns(new List<Transfer> 
                {
                    new Transfer { Amount = 100, ReceiverId = otherUser, SenderId = currentUser }
                });

            var result = transactionService.GetSummaryForUser(currentUser);

            Assert.AreEqual(new Settlement { Amount = 100, UserId = otherUser }, result.First());
        }

        [TestMethod]
        public void GetSummaryForUser_NoExpenses_OnlySentTwoTransfersToOneUser()
        {
            var currentUser = "currentUser";
            var otherUser = "otherUser";

            transferRepository
                .Setup(x => x.GetAllSentAndReceived(currentUser))
                .Returns(new List<Transfer> 
                {
                    new Transfer { Amount = 50, ReceiverId = otherUser, SenderId = currentUser },
                    new Transfer { Amount = 50, ReceiverId = otherUser, SenderId = currentUser }
                });

            var result = transactionService.GetSummaryForUser(currentUser);

            Assert.AreEqual(new Settlement { Amount = 100, UserId = otherUser }, result.First());
        }

        [TestMethod]
        public void GetSummaryForUser_NoExpenses_OnlySentThreeTransfersToTwoUsers()
        {
            var currentUser = "currentUser";
            var otherUser1 = "otherUser1";
            var otherUser2 = "otherUser2";

            transferRepository
                .Setup(x => x.GetAllSentAndReceived(currentUser))
                .Returns(new List<Transfer> 
                {
                    new Transfer { Amount = 50, ReceiverId = otherUser1, SenderId = currentUser },
                    new Transfer { Amount = 50, ReceiverId = otherUser1, SenderId = currentUser },
                    new Transfer { Amount = 66, ReceiverId = otherUser2, SenderId = currentUser }
                });

            var result = transactionService.GetSummaryForUser(currentUser);

            CollectionAssert.AreEqual(
                new List<Settlement>
                {
                    new Settlement { Amount = 100, UserId = otherUser1 },
                    new Settlement { Amount = 66, UserId = otherUser2 }
                },
                result.ToList());
        }

        [TestMethod]
        public void GetSummaryForUser_NoExpenses_SentThreeTransfersToTwoUsersOneReceived()
        {
            var currentUser = "currentUser";
            var otherUser1 = "otherUser1";
            var otherUser2 = "otherUser2";

            transferRepository
                .Setup(x => x.GetAllSentAndReceived(currentUser))
                .Returns(new List<Transfer> 
                {
                    new Transfer { Amount = 50, ReceiverId = otherUser1, SenderId = currentUser },
                    new Transfer { Amount = 50, ReceiverId = otherUser1, SenderId = currentUser },
                    new Transfer { Amount = 66, ReceiverId = otherUser2, SenderId = currentUser },
                    new Transfer { Amount = 40, ReceiverId = currentUser, SenderId = otherUser2}
                });

            var result = transactionService.GetSummaryForUser(currentUser);

            CollectionAssert.AreEqual(
                new List<Settlement>
                {
                    new Settlement { Amount = 100, UserId = otherUser1 },
                    new Settlement { Amount = 26, UserId = otherUser2 }
                },
                result.ToList());
        }

        [TestMethod]
        public void GetSummaryForUser_PaidExpenses_SentThreeTransfersToTwoUsersOneReceived()
        {
            var currentUser = "currentUser";
            var otherUser1 = "otherUser1";
            var otherUser2 = "otherUser2";

            transferRepository
                .Setup(x => x.GetAllSentAndReceived(currentUser))
                .Returns(new List<Transfer> 
                {
                    new Transfer { Amount = 50, ReceiverId = otherUser1, SenderId = currentUser },
                    new Transfer { Amount = 50, ReceiverId = otherUser1, SenderId = currentUser },
                    new Transfer { Amount = 66, ReceiverId = otherUser2, SenderId = currentUser },
                    new Transfer { Amount = 40, ReceiverId = currentUser, SenderId = otherUser2}
                });
            expensesRepository
                .Setup(x => x.GetAll(currentUser))
                .Returns(new List<Expense>
                {
                    new Expense
                    { 
                        Amount = 30, UserPayingId = currentUser, Participants = new List<User>
                        {
                            new User { Id = currentUser },
                            new User { Id = otherUser1 },
                            new User { Id = otherUser2 }
                        }
                    }
                });

            var result = transactionService.GetSummaryForUser(currentUser);

            CollectionAssert.AreEqual(
                new List<Settlement>
                {
                    new Settlement { Amount = 110, UserId = otherUser1 },
                    new Settlement { Amount = 36, UserId = otherUser2 }
                },
                result.ToList());
        }

        [TestMethod]
        public void GetSummaryForUser_PaidExpensesAndReceived_SentThreeTransfersToTwoUsersOneReceived()
        {
            var currentUser = "currentUser";
            var otherUser1 = "otherUser1";
            var otherUser2 = "otherUser2";

            transferRepository
                .Setup(x => x.GetAllSentAndReceived(currentUser))
                .Returns(new List<Transfer> 
                {
                    new Transfer { Amount = 50, ReceiverId = otherUser1, SenderId = currentUser },
                    new Transfer { Amount = 50, ReceiverId = otherUser1, SenderId = currentUser },
                    new Transfer { Amount = 66, ReceiverId = otherUser2, SenderId = currentUser },
                    new Transfer { Amount = 40, ReceiverId = currentUser, SenderId = otherUser2}
                });
            expensesRepository
                .Setup(x => x.GetAll(currentUser))
                .Returns(new List<Expense>
                {
                    new Expense
                    {
                        Amount = 30, UserPayingId = currentUser, Participants = new List<User> 
                        {
                            new User { Id = currentUser },
                            new User { Id = otherUser1 },
                            new User { Id = otherUser2 }
                        }
                    },
                    new Expense 
                    { 
                        Amount = 75, UserPayingId = otherUser1, Participants = new List<User> 
                        {
                            new User { Id = currentUser },
                            new User { Id = otherUser1 },
                            new User { Id = otherUser2 }
                        }
                    }
                });

            var result = transactionService.GetSummaryForUser(currentUser);

            CollectionAssert.AreEqual(
                new List<Settlement>
                {
                    new Settlement { Amount = 85, UserId = otherUser1 },
                    new Settlement { Amount = 36, UserId = otherUser2 }
                },
                result.ToList());
        }

        [TestMethod]
        public void GetSummaryForUser_PaidExpensesAndReceived_SentThreeTransfersToTwoUsersOneReceived_TwoGroups_OneFiltered()
        {
            var currentUser = "currentUser";
            var otherUser1 = "otherUser1";
            var otherUser2 = "otherUser2";
            var filteredGroup = 1;
            transferRepository
                .Setup(x => x.GetAllSentAndReceived(currentUser, filteredGroup))
                .Returns(new List<Transfer> 
                {
                    new Transfer { Amount = 50, ReceiverId = otherUser1, SenderId = currentUser, GroupId = filteredGroup },
                    new Transfer { Amount = 66, ReceiverId = otherUser2, SenderId = currentUser, GroupId = filteredGroup }
                });
            expensesRepository
                .Setup(x => x.GetAll(currentUser, filteredGroup))
                .Returns(new List<Expense>
                {
                    new Expense
                    {
                        Amount = 30, UserPayingId = currentUser, Participants = new List<User> 
                        {
                            new User { Id = currentUser },
                            new User { Id = otherUser1 },
                            new User { Id = otherUser2 }
                        },
                        GroupId = filteredGroup
                    }                   
                });

            var result = transactionService.GetSummaryForUser(currentUser, filteredGroup);

            CollectionAssert.AreEqual(
                new List<Settlement>
                {
                    new Settlement { Amount = 60, UserId = otherUser1 },
                    new Settlement { Amount = 76, UserId = otherUser2 }
                },
                result.ToList());
        }

        [TestMethod]
        public void AddExpense()
        {
            var groupId = 1;
            var userId = "user";
            var group = new Group { GroupId = groupId, Members = new List<User> { new User { Id = userId } } };
            var description = "desc";
            var date = new DateTime(2015, 1, 1);
            var amount = 15.11;
            var participants = new List<string> { "1", "2" };
            var expectedExpense = new Expense
            {
                Amount = amount,
                Date = date,
                Description = description,
                GroupId = groupId,
                UserPayingId = userId
            };
            groupService.Setup(x => x.GetGroup(groupId)).Returns(group);

            transactionService.AddExpense(userId, groupId, description, date, amount, participants);

            expensesRepository.Verify(x => x.Add(expectedExpense), Times.Once());
            expensesRepository.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void EditExpense_TheSameGroup()
        {
            var expenseId = 1;
            var userId = "user";
            var groupId = 1;
            var description = "desc";
            var date = new DateTime(2015, 1, 1);
            var amount = 15.11;
            var participants = new List<string> { "1", "2" };
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
            expensesRepository.Setup(x => x.Get(expenseId)).Returns(expenseOriginal);
            userRepository.Setup(x => x.GetUser(It.IsAny<string>())).Returns(new User { Id = "1" });

            transactionService.EditExpense(userId, expenseId, groupId, description, date, amount, participants);

            expensesRepository.Verify(x => x.Update(expectedExpense), Times.Once());
            expensesRepository.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void EditExpense_GroupChanged()
        {
            var expenseId = 1;
            var userId = "user";
            var groupId = 1;
            var description = "desc";
            var date = new DateTime(2015, 1, 1);
            var amount = 15.11;
            var participants = new List<string> { "1", "2" };
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
            expensesRepository.Setup(x => x.Get(expenseId)).Returns(expenseOriginal);
            groupService.Setup(x => x.GetGroup(groupId)).Returns(new Group { GroupId = groupId });
            userRepository.Setup(x => x.GetUser(It.IsAny<string>())).Returns(new User { Id = "1" });

            transactionService.EditExpense(userId, expenseId, groupId, description, date, amount, participants);

            expensesRepository.Verify(x => x.Update(expectedExpense), Times.Once());
            expensesRepository.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void EditExpense_OtherUserRequest()
        {
            var expenseId = 1;
            var userId = "user";
            var groupId = 1;
            var description = "desc";
            var date = new DateTime(2015, 1, 1);
            var amount = 15.11;
            var participants = new List<string> { "1", "2" };
            var expenseOriginal = new Expense
            {
                Amount = 5,
                Date = new DateTime(2013, 1, 1),
                Description = "old",
                GroupId = groupId,
                UserPayingId = "otherUser",
                ExpenseId = expenseId
            };
            userRepository.Setup(x => x.GetUser(It.IsAny<string>())).Returns(new User { Id = "1" });            
            expensesRepository.Setup(x => x.Get(expenseId)).Returns(expenseOriginal);

            var result = transactionService.EditExpense(userId, expenseId, groupId, description, date, amount, participants);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveExpense()
        {
            var expenseId = 1;
            var userId = "user";
            var expense = new Expense { ExpenseId = expenseId, UserPayingId = userId };
            expensesRepository.Setup(x => x.Get(expenseId)).Returns(expense);

            transactionService.RemoveExpense(userId, expenseId);

            expensesRepository.Verify(x => x.Remove(expense), Times.Once());
            expensesRepository.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void RemoveExpense_OtherUserRequest()
        {
            var expenseId = 1;
            var expense = new Expense { ExpenseId = expenseId, UserPayingId = "otherUser" };
            expensesRepository.Setup(x => x.Get(expenseId)).Returns(expense);

            var result = transactionService.RemoveExpense("user", expenseId);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveTransfer()
        {
            var transferId = 1;
            var userId = "user";
            var transfer = new Transfer { TransferId = transferId, SenderId = userId };
            transferRepository.Setup(x => x.Get(transferId)).Returns(transfer);

            transactionService.RemoveTransfer(userId, transferId);

            transferRepository.Verify(x => x.Remove(transfer), Times.Once());
            transferRepository.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void RemoveTransfer_OtherUserRequest()
        {
            var transferId = 1;
            var transfer = new Transfer { TransferId = transferId, SenderId = "otherUser" };
            transferRepository.Setup(x => x.Get(transferId)).Returns(transfer);

            var result = transactionService.RemoveTransfer("user", transferId);

            Assert.IsFalse(result);
        }
    }
}