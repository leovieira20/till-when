using System.Threading.Tasks;

namespace TillWhen.Domain.Aggregates.WorkableAggregate;

public interface IWorkableRepository
{
    void Add(Workable workable);
    Task CommitAsync();
}