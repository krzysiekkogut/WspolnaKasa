using System.Collections.Generic;
using DataAccessLayer.Entities.ExpensesDomain;

namespace ExpensesDomain.Repositories
{
    public interface IExpensesRepository
    {
        IEnumerable<Expense> GetAll(string userId);

        IEnumerable<Expense> GetAll(string userId, int groupId);
        
        Expense Get(int expenseId);

        void Add(Expense expense);

        void SaveChanges();

        void Remove(Expense expense);

        void Update(Expense expense);
    }
}
