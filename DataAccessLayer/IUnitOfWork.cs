using Domain.Entities;

namespace DataAccessLayer
{
    public interface IUnitOfWork
    {
        Repository<Group, int> GroupsRepository { get; }
        Repository<Expense, int> ExpensesRepository { get; }
        Repository<Transfer, int> TransfersRepository { get; }
        Repository<User, string> UsersRepository { get; }
        void SaveChanges();
    }
}
