using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Interfaces;
using System.Linq.Expressions;

namespace PaymentCard.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        protected DbContext _dataContext;
        protected DbSet<TEntity> _dataTable;

        public GenericRepository(DbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException("WriteDbContext");
            _dataTable = _dataContext.Set<TEntity>();
        }

        public virtual void Create(params IEnumerable<TEntity> items)
        {
            foreach (TEntity item in items)
            {
                _dataContext.Add(item);
            }
        }

        public virtual void Update(params IEnumerable<TEntity> items)
        {
            foreach (TEntity item in items)
            {
                _dataContext.Update(item);
            }
        }

        public virtual void Delete(params IEnumerable<TEntity> items)
        {
            foreach (TEntity item in items)
            {
                _dataContext.Remove(item);
            }
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dataTable.ToListAsync();
        }

        public virtual async Task<TEntity> FindAsync(int id)
        {
            return await _dataTable.SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IEnumerable<TEntity>> SearchForAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dataTable.Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllIncludeAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties)
        {
            return await GetAllIncluding(includeProperties).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> SearchForIncludeAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            return await query.Where(predicate).ToListAsync();
        }

        private IQueryable<TEntity> GetAllIncluding(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties)
        {
            IQueryable<TEntity> queryable = _dataTable;
            queryable = includeProperties(queryable);
            return queryable;
        }
    }
}