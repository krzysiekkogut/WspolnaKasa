using System.Collections.Generic;
using DataAccessLayer.Entities.ExpensesDomain;
using ExpensesDomain.DomainModel;
using ExpensesDomain.Repositories;
using System.Linq;
using DataAccessLayer;
using System;

namespace ExpensesDomain.Services
{
    public class TransactionService : ITransactionService
    {
        private IExpensesRepository _expensesRepository;
        private ITransferRepository _transferRepository;
        private IGroupService _groupService;
        private IApplicationUserRepository _userRepository;

        public TransactionService(
            IExpensesRepository expensesRepository,
            ITransferRepository transferRepository,
            IGroupService groupService,
            IApplicationUserRepository userRepository)
        {
            _expensesRepository = expensesRepository;
            _transferRepository = transferRepository;
            _groupService = groupService;
            _userRepository = userRepository;
        }

        public Expense GetExpense(int id)
        {
            return _expensesRepository.Get(id);
        }

        public IEnumerable<Expense> GetAllExpenses(string userId)
        {
            return _expensesRepository.GetAll(userId);
        }

        public IEnumerable<Expense> GetAllExpenses(string userId, int groupId)
        {
            return _expensesRepository.GetAll(userId, groupId);
        }

        public IEnumerable<Transfer> GetAllSentAndReceivedTransfers(string userId)
        {
            return _transferRepository.GetAllSentAndReceived(userId);
        }

        public IEnumerable<Transfer> GetAllSentAndReceivedTransfers(string userId, int groupId)
        {
            return _transferRepository.GetAllSentAndReceived(userId, groupId);
        }

        public IEnumerable<Settlement> GetSummaryForUser(string userId)
        {
            var transfers = _transferRepository.GetAllSentAndReceived(userId);
            var expenses = _expensesRepository.GetAll(userId);

            return GroupTransfersAndExpensesToSummary(userId, transfers, expenses);
        }

        public IEnumerable<Settlement> GetSummaryForUser(string userId, int groupId)
        {
            var transfers = _transferRepository.GetAllSentAndReceived(userId, groupId);
            var expenses = _expensesRepository.GetAll(userId, groupId);

            return GroupTransfersAndExpensesToSummary(userId, transfers, expenses);
        }

        private IEnumerable<Settlement> GroupTransfersAndExpensesToSummary(string userId, IEnumerable<Transfer> transfers, IEnumerable<Expense> expenses)
        {
            var transfersSent = transfers
                .Where(t => t.ApplicationUserId == userId)
                .GroupBy(t => t.ReceiverId)
                .Select(tg => new Settlement { Amount = tg.Sum(s => s.Amount), UserId = tg.Key });

            var transfersReceived = transfers
                .Where(t => t.ReceiverId == userId)
                .GroupBy(t => t.ApplicationUserId)
                .Select(tg => new Settlement { Amount = -tg.Sum(s => s.Amount), UserId = tg.Key });

            var expensesPaid = expenses
                .Where(x => x.UserPayingId == userId);
            List<Settlement> expensesPaidSettlements = new List<Settlement>();
            foreach (var exp in expensesPaid)
            {
                var amount = exp.Amount / exp.Participants.Count;
                foreach (var set in exp.Participants.Where(x => x.Id != userId))
                {
                    expensesPaidSettlements.Add(new Settlement { Amount = amount, UserId = set.Id });
                }
            }

            var expensesParticipated = expenses
                .Where(x => x.Participants.Select(y => y.Id).Contains(userId))
                .Except(expensesPaid)
                .Select(x => new Settlement { UserId = x.UserPayingId, Amount = -x.Amount / x.Participants.Count });

            return
                 transfersSent
                .Concat(transfersReceived)
                .Concat(expensesPaidSettlements)
                .Concat(expensesParticipated)
                .GroupBy(x => x.UserId)
                .Select(x => new Settlement { Amount = x.Sum(y => y.Amount), UserId = x.Key });
        }

        public void AddExpense(string userId, int groupId, string description, DateTime date, double amount, IEnumerable<string> participants)
        {
            var group = _groupService.GetGroup(groupId);
            var expense = new Expense
            {
                UserPayingId = userId,
                GroupId = groupId,
                Group = group,
                Description = description,
                Date = date,
                Amount = amount,
            };
            expense.Participants = participants.Select(x => _userRepository.GetUser(x)).ToList();
            _expensesRepository.Add(expense);
            _expensesRepository.SaveChanges();
        }

        public bool EditExpense(string userId, int expenseId, int groupId, string description, DateTime date, double amount, IEnumerable<string> participants)
        {
            var expense = _expensesRepository.Get(expenseId);

            if (expense.UserPayingId != userId) return false;

            if (expense.GroupId != groupId)
            {
                expense.Group = _groupService.GetGroup(groupId);
                expense.GroupId = groupId;
            }
            expense.Description = description;
            expense.Date = date;
            expense.Amount = amount;

            expense.Participants.AddRange(participants.Select(x => _userRepository.GetUser(x)));
            expense.Participants.RemoveAll(x => participants.Contains(x.Id) == false);

            _expensesRepository.Update(expense);
            _expensesRepository.SaveChanges();

            return true;
        }

        public bool RemoveExpense(string userId, int expenseId)
        {
            var expense = _expensesRepository.Get(expenseId);

            if (expense.UserPayingId != userId) return false;

            _expensesRepository.Remove(expense);
            _expensesRepository.SaveChanges();

            return true;
        }

        public void AddTransfer(string userFrom, string userTo, int groupId, string description, DateTime date, double amount)
        {
            var group = _groupService.GetGroup(groupId);
            var transfer = new Transfer
            {
                Amount = amount,
                ApplicationUser = _userRepository.GetUser(userFrom),
                ApplicationUserId = userFrom,
                Date = date,
                Description = description,
                Group = group,
                GroupId = groupId,
                ReceiverId = userTo
            };
            _transferRepository.Add(transfer);
            _transferRepository.SaveChanges();
        }

        public bool RemoveTransfer(string userId, int transferId)
        {
            var transfer = _transferRepository.Get(transferId);

            if (transfer.ApplicationUserId != userId) return false;

            _transferRepository.Remove(transfer);
            _transferRepository.SaveChanges();

            return true;
        }
    }
}
