using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;

namespace TillWhen.Database.SqlServer.Repositories;

public class EfWorkableQueueRepository : IWorkableQueueRepository
{
    private readonly TillWhenContext _context;

    public EfWorkableQueueRepository(TillWhenContext context)
    {
        _context = context;
    }

    public Task<WorkableQueue?> GetAsync(Guid requestQueueId)
    {
        return _context
            .WorkableQueues
            .Include(x => x.Workables)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == requestQueueId);
    }
    
    public void Create(WorkableQueue queue)
    {
        _context.WorkableQueues.Add(queue);
    }

    public Task CommitAsync()
    {
        return _context.SaveChangesAsync();
    }
}