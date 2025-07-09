using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure;
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IDbContextFactory<MainDbContext> _contextFactory;
    private MainDbContext? _dbContext;
    private IDbContextTransaction? _transaction;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(IDbContextFactory<MainDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    private MainDbContext DbContext
        => _dbContext ??= _contextFactory.CreateDbContext();

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (!_repositories.TryGetValue(type, out var repo))
        {
            repo = new GenericRepository<TEntity>(DbContext);
            _repositories[type] = repo;
        }

        return (IGenericRepository<TEntity>)repo;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => DbContext.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            return;

        _transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction.");

        await SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _dbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}
