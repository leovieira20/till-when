using TillWhen.Domain.Aggregates.QueueAggregate;

namespace TillWhen.Database.SqlServer.Repositories;

public class EfRepositoryBase : IRepositoryBase
{
    protected readonly TillWhenContext Context;

    protected EfRepositoryBase(TillWhenContext context)
    {
        Context = context;
    }

    public void Create<T>(T entity)
    {
        Context.Add(entity!);
    }

    public Task SaveAsync()
    {
        return Context.SaveChangesAsync();
    }
}