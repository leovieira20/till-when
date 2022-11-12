using TillWhen.Domain.Aggregates.WorkableAggregate;

namespace TillWhen.Database.SqlServer.Repositories;

internal class EfWorkableRepository : EfRepositoryBase, IWorkableRepository
{
    public EfWorkableRepository(TillWhenContext context) 
        : base(context)
    {
    }
}