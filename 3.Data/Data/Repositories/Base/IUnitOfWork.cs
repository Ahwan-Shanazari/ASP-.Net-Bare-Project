namespace Data.Repositories.Base;

public interface IUnitOfWork
{
    int CommitChanges();
    Task<int> CommitChangesAsync();
}