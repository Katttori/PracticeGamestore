namespace PracticeGamestore.DataAccess.UnitOfWork;

public interface IUnitOfWork
{ 
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}