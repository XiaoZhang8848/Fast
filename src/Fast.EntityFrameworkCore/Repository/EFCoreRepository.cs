using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Fast.DependencyInjection;
using Fast.EntityFrameworkCore.Data;
using Fast.Infrastructure.Dtos;
using Fast.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fast.EntityFrameworkCore.Repository;

public class EFCoreRepository<TEntity> : IEFCoreRepository<TEntity>, ITransientTag
    where TEntity : Entity
{
    private readonly FastDbContext _dbContext;
    private readonly DbSet<TEntity> _entity;

    public EFCoreRepository(FastDbContext dbContext)
    {
        _dbContext = dbContext;
        _entity = _dbContext.Set<TEntity>();
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        return await _entity.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(expression, cancellationToken);
        if (entity == null)
        {
            throw new Exception("数据不存在");
        }

        return entity;
    }

    public async Task InsertAsync(TEntity entity, bool isSave = false, CancellationToken cancellationToken = default)
    {
        await _entity.AddAsync(entity, cancellationToken);
        if (isSave)
        {
            await SaveAsync(cancellationToken);
        }
    }

    public async Task DeleteAsync(TEntity entity, bool isSave = false, CancellationToken cancellationToken = default)
    {
        _entity.Remove(entity);
        if (isSave)
        {
            await SaveAsync(cancellationToken);
        }
    }

    public async Task UpdateAsync(TEntity entity, bool isSave = false, CancellationToken cancellationToken = default)
    {
        _entity.Update(entity);
        if (isSave)
        {
            await SaveAsync(cancellationToken);
        }
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, PageInput input,
        CancellationToken cancellationToken = default)
    {
        return await _entity.Where(expression)
            .OrderBy(input.Sort)
            .Skip(input.SkipCount).Take(input.TakeCount)
            .ToListAsync(cancellationToken);
    }
}