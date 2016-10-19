using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Domain.Entities;

namespace DataAccessLayer.Repositories
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
            var user = _dbContext.Users.Find(userId);
            return user.ExpensesPaid.Union(user.ExpensesParticipated).OrderByDescending(e => e.Date);
        }

        public IEnumerable<Expense> GetAll(string userId, int groupId)
        {
            return GetAll(userId).Where(e => e.GroupId == groupId);
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
