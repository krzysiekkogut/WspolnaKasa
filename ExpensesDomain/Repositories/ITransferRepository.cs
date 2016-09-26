using System.Collections.Generic;
using DataAccessLayer.Entities.ExpensesDomain;

namespace ExpensesDomain.Repositories
{
    public interface ITransferRepository
    {
        IEnumerable<Transfer> GetAllSentAndReceived(string userId);

        IEnumerable<Transfer> GetAllSentAndReceived(string userId, int groupId);

        void SaveChanges();

        void Add(Transfer transfer);

        Transfer Get(int transferId);

        void Remove(Transfer transfer);
    }
}
