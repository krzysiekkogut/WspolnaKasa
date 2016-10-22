using System.Linq;

namespace DataAccessLayer
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        TEntity Get(TId id);
        IQueryable<TEntity> GetAll();
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}