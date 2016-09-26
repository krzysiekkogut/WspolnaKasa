using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.Entities.ExpensesDomain;

namespace ExpensesDomain.Repositories
{
    public class ExpensesRepository : IExpensesRepository
    {
        private ApplicationDbContext _dbContext;

        public ExpensesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public IEnumerable<Expense> GetAll(string userId)
        {
            return _dbContext.Users
                .Find(userId)
                .Expenses
                .Union(_dbContext.Expenses.Where(x => x.UserPayingId == userId))
                .OrderByDescending(e => e.Date);
        }

        public IEnumerable<Expense> GetAll(string userId, int groupId)
        {
            return _dbContext.Users
                .Find(userId)
                .Expenses
                .Union(_dbContext.Expenses.Where(x => x.UserPayingId == userId))
                .Where(e => e.GroupId == groupId)
                .OrderByDescending(e => e.Date);
        }

        public Expense Get(int expenseId)
        {
            return _dbContext.Set<Expense>().Find(expenseId);
        }

        public void Add(Expense expense)
        {
            _dbContext.Set<Expense>().Add(expense);
        }

        public void Remove(Expense expense)
        {
            _dbContext.Set<Expense>().Remove(expense);
        }

        public void Update(Expense expense)
        {
            _dbContext.Entry(expense).State = EntityState.Modified;
        }
    }
}
