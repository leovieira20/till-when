using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;

namespace TillWhen.Database.SqlServer.Repositories;

public class EfTaskQueueRepository : ITaskQueueRepository
{
    private readonly TillWhenContext _context;

    public EfTaskQueueRepository(TillWhenContext context)
    {
        _context = context;
    }

    public Task<TaskQueue?> GetAsync(Guid requestQueueId)
    {
        return _context
            .TaskQueues
            .Include(x => x.Projects)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == requestQueueId);
    }

    public Task<List<Project>> GetProjectsForQueueAsync()
    {
        throw new NotImplementedException();
    }

    public void Create(TaskQueue queue)
    {
        _context.TaskQueues.Add(queue);
    }

    public Task CommitAsync()
    {
        return _context.SaveChangesAsync();
    }
}