using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
    {
        private ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity Get(TId id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
