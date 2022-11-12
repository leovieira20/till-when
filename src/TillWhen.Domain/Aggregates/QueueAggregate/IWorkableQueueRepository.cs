using System;
using System.Threading.Tasks;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public interface IWorkableQueueRepository : IRepositoryBase
{
    Task<WorkableQueue?> GetAsync(Guid requestQueueId);
}