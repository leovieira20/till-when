using System;
using System.Threading.Tasks;
using TillWhen.Domain.Aggregates.QueueAggregate;

namespace TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;

public interface IWorkableQueueConfigurationRepository : IRepositoryBase
{
    Task<WorkableQueueConfiguration?> GetByQueueIdAsync(Guid queueId);
    Task<WorkableQueueConfiguration?> GetByIdAsync(Guid configurationId);
}