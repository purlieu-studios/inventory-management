using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TEntity>() ?? throw new InvalidOperationException("DbSet cannot be null");
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dbSet.AddAsync(entity);
    }

    public virtual Task DeleteAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
    {
        var query = BuildQuery(predicate, include, disableTracking, ignoreQueryFilters);
        return orderBy != null
            ? await orderBy(query).FirstOrDefaultAsync()
            : await query.FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> WhereAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
    {
        var query = BuildQuery(predicate, include, disableTracking, ignoreQueryFilters);
        return orderBy != null
            ? await orderBy(query).ToListAsync()
            : await query.ToListAsync();
    }

    public virtual Task<IEnumerable<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
        => WhereAsync(null, orderBy, include, disableTracking, ignoreQueryFilters);

    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        => predicate == null ? _dbSet.CountAsync() : _dbSet.CountAsync(predicate);

    public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? selector = null)
        => selector == null ? _dbSet.AnyAsync() : _dbSet.AnyAsync(selector);

    public void ChangeEntityState(TEntity entity, EntityState state)
        => _dbContext.Entry(entity).State = state;

    public Task SaveAsync() => _dbContext.SaveChangesAsync();

    protected IQueryable<TEntity> BuildQuery(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool disableTracking,
        bool ignoreQueryFilters)
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
            query = query.AsNoTracking();

        if (include != null)
            query = include(query) ?? query;

        if (predicate != null)
            query = query.Where(predicate);

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        return query;
    }
}
