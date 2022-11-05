using TillWhen.Domain.Aggregates.WorkableAggregate;

namespace TillWhen.Database.SqlServer.Repositories;

internal class EfWorkableRepository : IWorkableRepository
{
    private readonly TillWhenContext _context;

    public EfWorkableRepository(TillWhenContext context)
    {
        _context = context;
    }
    
    public void Add(Workable workable)
    {
        _context.Workables.Add(workable);
    }
    
    public Task CommitAsync()
    {
        return _context.SaveChangesAsync();
    }
}