using System;
using System.Collections.Generic;
using DataAccessLayer.Entities.ExpensesDomain;
using ExpensesDomain.DomainModel;

namespace ExpensesDomain.Services
{
    public interface ITransactionService
    {
        Expense GetExpense(int id);

        IEnumerable<Expense> GetAllExpenses(string userId);

        IEnumerable<Expense> GetAllExpenses(string userId, int groupId);

        IEnumerable<Transfer> GetAllSentAndReceivedTransfers(string userId);

        IEnumerable<Transfer> GetAllSentAndReceivedTransfers(string userId, int groupId);

        IEnumerable<Settlement> GetSummaryForUser(string userId);

        IEnumerable<Settlement> GetSummaryForUser(string userId, int groupId);

        void AddExpense(string userId, int groupId, string description, DateTime date, double amount, IEnumerable<string> participants);

        bool EditExpense(string userId, int expenseId, int groupId, string description, DateTime date, double amount, IEnumerable<string> participants);

        bool RemoveExpense(string userId, int expenseId);

        void AddTransfer(string userFrom, string userTo, int groupId, string description, DateTime date, double amount);
    }
}
