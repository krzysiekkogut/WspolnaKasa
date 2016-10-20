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

        private Repository<Expense, int> _expensesRepository;
        public Repository<Expense, int> ExpensesRepository
        {
            get
            {
                return _expensesRepository ?? (_expensesRepository = new Repository<Expense, int>(_dbContext));
            }
        }

        private Repository<Group, int> _groupsRepository;
        public Repository<Group, int> GroupsRepository
        {
            get
            {
                return _groupsRepository ?? (_groupsRepository = new Repository<Group, int>(_dbContext));
            }
        }

        private Repository<Transfer, int> _transfersRepository;
        public Repository<Transfer, int> TransfersRepository
        {
            get
            {
                return _transfersRepository ?? (_transfersRepository = new Repository<Transfer, int>(_dbContext));
            }
        }

        private Repository<User, string> _usersRepository;
        public Repository<User, string> UsersRepository
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
