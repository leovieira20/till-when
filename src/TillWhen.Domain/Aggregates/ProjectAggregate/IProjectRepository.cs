using System.Threading.Tasks;

namespace TillWhen.Domain.Aggregates.ProjectAggregate;

public interface IProjectRepository
{
    void Add(Project project);
    Task CommitAsync();
}