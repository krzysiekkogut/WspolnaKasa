using Domain.Entities;

namespace DataAccessLayer
{
    public interface IUnitOfWork
    {
        IRepository<Group, int> GroupsRepository { get; }
        IRepository<Expense, int> ExpensesRepository { get; }
        IRepository<Transfer, int> TransfersRepository { get; }
        IRepository<User, string> UsersRepository { get; }
        void SaveChanges();
    }
}
