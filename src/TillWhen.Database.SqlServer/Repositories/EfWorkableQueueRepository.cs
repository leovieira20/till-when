using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Aggregates.QueueAggregate;

namespace TillWhen.Database.SqlServer.Repositories;

public class EfWorkableQueueRepository : EfRepositoryBase, IWorkableQueueRepository
{
    public EfWorkableQueueRepository(TillWhenContext context) : base(context)
    {
    }

    public Task<WorkableQueue?> GetAsync(Guid requestQueueId)
    {
        return Context
            .WorkableQueues
            .Include(x => x.Workables)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public Task CommitAsync()
    {
        return Context.SaveChangesAsync();
    }
}