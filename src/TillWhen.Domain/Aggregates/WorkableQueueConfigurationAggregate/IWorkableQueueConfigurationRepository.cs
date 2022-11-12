using System;
using System.Threading.Tasks;

namespace TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;

public interface IWorkableQueueConfigurationRepository
{
    Task<WorkableQueueConfiguration?> GetAsync(Guid workableQueueId);
}