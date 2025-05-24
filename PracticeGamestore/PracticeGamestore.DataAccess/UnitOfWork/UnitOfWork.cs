namespace PracticeGamestore.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private GamestoreDbContext _dbContext;
    
    public UnitOfWork(GamestoreDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
       return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}