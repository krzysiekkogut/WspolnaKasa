using System.Collections.Generic;
using System.Linq;
using System;
using Domain.Entities;
using Domain.Models;
using DataAccessLayer;
using Domain.Exceptions;

namespace Domain.Services
{
    public class TransactionService : ITransactionService
    {
        private IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Expense GetExpense(int id)
        {
            return _unitOfWork.ExpensesRepository.Get(id);
        }

        public IEnumerable<Expense> GetAllExpenses(string userId)
        {
            var user = _unitOfWork.UsersRepository.Get(userId);
            return user.ExpensesPaid.Union(user.ExpensesParticipated).OrderByDescending(e => e.Date);
        }

        public IEnumerable<Expense> GetAllExpenses(string userId, int groupId)
        {
            return GetAllExpenses(userId).Where(e => e.GroupId == groupId);
        }

        public IEnumerable<Transfer> GetAllTransfers(string userId)
        {
            var user = _unitOfWork.UsersRepository.Get(userId);
            return user.TransfersReceived.Union(user.TransfersSent).OrderByDescending(t => t.Date);
        }

        public IEnumerable<Transfer> GetAllTransfers(string userId, int groupId)
        {
            return GetAllTransfers(userId).Where(t => t.GroupId == groupId);
        }

        public IEnumerable<Settlement> GetSummaryForUser(string userId)
        {
            var user = _unitOfWork.UsersRepository.Get(userId);
            var transfersSent = user.TransfersSent;
            var transfersReceived = user.TransfersReceived;
            var expensesPaid = user.ExpensesPaid;
            var expensesParticipated = user.ExpensesParticipated;
            return GroupTransfersAndExpensesToSettlements(userId, transfersSent, transfersReceived, expensesPaid, expensesParticipated);
        }

        public IEnumerable<Settlement> GetSummaryForUser(string userId, int groupId)
        {
            var user = _unitOfWork.UsersRepository.Get(userId);
            var transfersSent = user.TransfersSent.Where(t => t.GroupId == groupId);
            var transfersReceived = user.TransfersReceived.Where(t => t.GroupId == groupId);
            var expensesPaid = user.ExpensesPaid.Where(e => e.GroupId == groupId);
            var expensesParticipated = user.ExpensesParticipated.Where(e => e.GroupId == groupId);
            return GroupTransfersAndExpensesToSettlements(userId, transfersSent, transfersReceived, expensesPaid, expensesParticipated);
        }

        private IEnumerable<Settlement> GroupTransfersAndExpensesToSettlements(
            string userId, IEnumerable<Transfer> transfersSent,
            IEnumerable<Transfer> transfersReceived,
            IEnumerable<Expense> expensesPaid,
            IEnumerable<Expense> expensesParticipated)
        {
            var sentTransfersPart =
                    transfersSent
                        .GroupBy(t => t.ReceiverId)
                        .Select(tg => new Settlement
                        {
                            Amount = tg.Sum(t => t.Amount),
                            User = tg.First().Receiver
                        });

            var receivedTransfersPart =
                    transfersReceived
                        .GroupBy(t => t.SenderId)
                        .Select(tg => new Settlement
                        {
                            Amount = -tg.Sum(t => t.Amount),
                            User = tg.First().Sender
                        });

            var expensesPaidPart = new List<Settlement>();
            foreach (var expense in expensesPaid)
            {
                var amount = expense.Amount / expense.Participants.Count;
                foreach (var participant in expense.Participants.Where(x => x.Id != userId))
                {
                    expensesPaidPart.Add(new Settlement
                    {
                        Amount = amount,
                        User = participant
                    });
                }
            }

            var expensesParticipatedPart = expensesParticipated
                .Except(expensesPaid)
                .Select(e => new Settlement
                {
                    Amount = -e.Amount / e.Participants.Count,
                    User = e.UserPaying
                });

            return
                 sentTransfersPart
                .Concat(receivedTransfersPart)
                .Concat(expensesPaidPart)
                .Concat(expensesParticipatedPart)
                .GroupBy(s => s.User.Id)
                .Select(ts => new Settlement
                {
                    Amount = ts.Sum(s => s.Amount),
                    User = ts.First().User
                });
        }

        public void AddExpense(string userId, int groupId, string description, DateTime date, double amount, IEnumerable<string> participants)
        {
            var group = _unitOfWork.GroupsRepository.Get(groupId);

            if (!CheckIfParticipantsAreGroupMembers(participants, group))
            {
                throw new ExpenseParticipantsMustBeGroupMembersExecption();
            }

            var expense = new Expense
            {
                UserPayingId = userId,
                GroupId = groupId,
                Group = group,
                Description = description,
                Date = date,
                Amount = amount,
            };
            expense.Participants = participants.Select(u => _unitOfWork.UsersRepository.Get(u)).ToList();
            _unitOfWork.ExpensesRepository.Add(expense);
            _unitOfWork.SaveChanges();
        }

        private bool CheckIfParticipantsAreGroupMembers(IEnumerable<string> participants, Group group)
        {
            var membersId = group.Members.Select(m => m.Id).OrderBy(id => id);
            return participants.All(p => membersId.Contains(p));
        }

        public void EditExpense(string userId, int expenseId, int groupId, string description, DateTime date, double amount, IEnumerable<string> participants)
        {
            var expense = _unitOfWork.ExpensesRepository.Get(expenseId);

            if (expense.UserPayingId != userId)
            {
                throw new CannotEditOtherUsersExpensesException();
            }

            if (expense.GroupId != groupId)
            {
                var group = _unitOfWork.GroupsRepository.Get(groupId);
                if (!CheckIfParticipantsAreGroupMembers(participants, group))
                {
                    throw new ExpenseParticipantsMustBeGroupMembersExecption();
                }
                expense.Group = group;
                expense.GroupId = groupId;
            }

            expense.Description = description;
            expense.Date = date;
            expense.Amount = amount;

            expense.Participants.AddRange(participants.Select(u => _unitOfWork.UsersRepository.Get(u)));
            expense.Participants.RemoveAll(x => !participants.Contains(x.Id));

            _unitOfWork.ExpensesRepository.Update(expense);
            _unitOfWork.SaveChanges();
        }

        public void RemoveExpense(string userId, int expenseId)
        {
            var expense = _unitOfWork.ExpensesRepository.Get(expenseId);

            if (expense.UserPayingId != userId)
            {
                throw new CannotEditOtherUsersExpensesException();
            }

            _unitOfWork.ExpensesRepository.Remove(expense);
            _unitOfWork.SaveChanges();
        }

        public void AddTransfer(string senderId, string receiverId, int groupId, string description, DateTime date, double amount)
        {
            var group = _unitOfWork.GroupsRepository.Get(groupId);
            var transfer = new Transfer
            {
                Amount = amount,
                Sender = _unitOfWork.UsersRepository.Get(senderId),
                SenderId = senderId,
                Date = date,
                Description = description,
                Group = group,
                GroupId = groupId,
                ReceiverId = receiverId,
                Receiver = _unitOfWork.UsersRepository.Get(receiverId)
            };
            _unitOfWork.TransfersRepository.Add(transfer);
            _unitOfWork.SaveChanges();
        }

        public void RemoveTransfer(string userId, int transferId)
        {
            var transfer = _unitOfWork.TransfersRepository.Get(transferId);

            if (transfer.SenderId != userId)
            {
                throw new CannotEditOtherUsersTransfersException();
            }

            _unitOfWork.TransfersRepository.Remove(transfer);
            _unitOfWork.SaveChanges();
        }
    }
}
