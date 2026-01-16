using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Interfaces;
using System.Linq.Expressions;

namespace PaymentCard.Data
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        void Create(params IEnumerable<TEntity> items);

        void Update(params IEnumerable<TEntity> items);

        void Delete(params IEnumerable<TEntity> items);

        Task SaveAsync();

        Task<TEntity> FindAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllIncludeAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties);

        Task<IEnumerable<TEntity>> SearchForAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> SearchForIncludeAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties);
    }
}