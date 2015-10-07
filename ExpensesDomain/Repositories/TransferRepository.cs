using System.Collections.Generic;
using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Entities.ExpensesDomain;
using System.Linq;

namespace ExpensesDomain.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        ApplicationDbContext _dbContext;
        public TransferRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Transfer> GetAllSentAndReceived(string userId)
        {
            return _dbContext.Users.Find(userId)
                .TransfersSent.Union(
                    _dbContext.Set<Transfer>()
                    .Where(t => t.ReceiverId == userId))
                .OrderByDescending(x => x.Date);
        }

        public IEnumerable<Transfer> GetAllSentAndReceived(string userId, int groupId)
        {
            return _dbContext.Users.Find(userId)
                .TransfersSent
                .Where(t => t.GroupId == groupId)
                .Union(
                    _dbContext.Set<Transfer>()
                    .Where(t => t.ReceiverId == userId && t.GroupId == groupId))
                .OrderByDescending(x => x.Date);
        }


        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Add(Transfer transfer)
        {
            _dbContext.Set<Transfer>().Add(transfer);
        }
    }
}
