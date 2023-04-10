using Data.Contexts;

namespace Data.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    public int CommitChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> CommitChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}