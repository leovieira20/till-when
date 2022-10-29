using System;
using System.Threading.Tasks;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public interface ITaskQueueRepository
{
    Task<TaskQueue?> GetAsync(Guid requestQueueId);
    void Create(TaskQueue queue);
    Task CommitAsync();
}