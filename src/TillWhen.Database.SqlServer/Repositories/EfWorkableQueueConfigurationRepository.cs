using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;

namespace TillWhen.Database.SqlServer.Repositories;

public class EfWorkableQueueConfigurationRepository : EfRepositoryBase, IWorkableQueueConfigurationRepository
{
    public EfWorkableQueueConfigurationRepository(TillWhenContext context) : base(context)
    {
    }

    public Task<WorkableQueueConfiguration?> GetAsync(Guid workableQueueId)
    {
        return Context
            .WorkableQueueConfigurations
            .FirstOrDefaultAsync();
    }
}