using System.Linq.Expressions;
using Fast.Infrastructure.Dtos;
using Fast.Infrastructure.Entities;

namespace Fast.EntityFrameworkCore.Repository;

public interface IEFCoreRepository<TEntity>
    where TEntity : Entity
{
    Task SaveAsync(CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, PageInput input,
        CancellationToken cancellationToken = default);

    Task InsertAsync(TEntity entity, bool isSave = false, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, bool isSave = false, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, bool isSave = false, CancellationToken cancellationToken = default);
}