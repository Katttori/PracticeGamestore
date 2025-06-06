using Microsoft.EntityFrameworkCore.Storage;

namespace PracticeGamestore.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private GamestoreDbContext _dbContext;
    private IDbContextTransaction? _transaction;
    
    public UnitOfWork(GamestoreDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
       return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
    
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            _transaction.Dispose();
            _transaction = null;
        }
    }
    
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            _transaction.Dispose();
            _transaction = null;
        }
    } 
}