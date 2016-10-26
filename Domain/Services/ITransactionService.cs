using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Services
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

        void EditExpense(string userId, int expenseId, int groupId, string description, DateTime date, double amount, IEnumerable<string> participants);

        void RemoveExpense(string userId, int expenseId);

        void AddTransfer(string userFrom, string userTo, int groupId, string description, DateTime date, double amount);

        void RemoveTransfer(string userId, int transferId);
    }
}
