using System.Threading.Tasks;
using TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public interface IRepositoryBase
{
    void Create <T>(T entity);
    void Update<T>(T configuration);
    Task SaveAsync();
}