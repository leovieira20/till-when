using System;
using System.Threading.Tasks;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public interface IWorkableQueueRepository
{
    Task<WorkableQueue?> GetAsync(Guid requestQueueId);
    void Create(WorkableQueue queue);
    Task CommitAsync();
}