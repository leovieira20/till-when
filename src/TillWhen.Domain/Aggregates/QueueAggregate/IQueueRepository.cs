using System;
using System.Threading.Tasks;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public interface IQueueRepository
{
    Task<TaskQueue?> GetAsync(Guid requestQueueId);
}