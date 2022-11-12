using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;

namespace TillWhen.Database.SqlServer.Repositories;

public class EfWorkableQueueConfigurationRepository : EfRepositoryBase, IWorkableQueueConfigurationRepository
{
    public EfWorkableQueueConfigurationRepository(TillWhenContext context) : base(context)
    {
    }

    public Task<WorkableQueueConfiguration?> GetByQueueIdAsync(Guid queueId)
    {
        return Context
            .WorkableQueueConfigurations
            .FirstOrDefaultAsync();
    }
    
    public Task<WorkableQueueConfiguration?> GetByIdAsync(Guid configurationId)
    {
        return Context
            .WorkableQueueConfigurations
            .FirstOrDefaultAsync(x => x.Id == configurationId);
    }
}