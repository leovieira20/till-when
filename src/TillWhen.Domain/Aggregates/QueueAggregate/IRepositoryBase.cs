using System.Threading.Tasks;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public interface IRepositoryBase
{
    void Create <T>(T entity);
    Task SaveAsync();
}