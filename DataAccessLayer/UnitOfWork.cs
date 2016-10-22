using System;
using Domain.Entities;

namespace DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IRepository<Expense, int> _expensesRepository;
        public IRepository<Expense, int> ExpensesRepository
        {
            get
            {
                return _expensesRepository ?? (_expensesRepository = new Repository<Expense, int>(_dbContext));
            }
        }

        private IRepository<Group, int> _groupsRepository;
        public IRepository<Group, int> GroupsRepository
        {
            get
            {
                return _groupsRepository ?? (_groupsRepository = new Repository<Group, int>(_dbContext));
            }
        }

        private IRepository<Transfer, int> _transfersRepository;
        public IRepository<Transfer, int> TransfersRepository
        {
            get
            {
                return _transfersRepository ?? (_transfersRepository = new Repository<Transfer, int>(_dbContext));
            }
        }

        private IRepository<User, string> _usersRepository;
        public IRepository<User, string> UsersRepository
        {
            get
            {
                return _usersRepository ?? (_usersRepository = new Repository<User, string>(_dbContext));
            }
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
