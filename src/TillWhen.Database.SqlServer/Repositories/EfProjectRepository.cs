using TillWhen.Domain.Aggregates.ProjectAggregate;

namespace TillWhen.Database.SqlServer.Repositories;

internal class EfProjectRepository : IProjectRepository
{
    private readonly TillWhenContext _context;

    public EfProjectRepository(TillWhenContext context)
    {
        _context = context;
    }
    
    public void Add(Project project)
    {
        _context.Projects.Add(project);
    }
    
    public Task CommitAsync()
    {
        return _context.SaveChangesAsync();
    }
}