using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;

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

    public void Update<T>(T configuration)
    {
        Context.Update(configuration!);
    }

    public Task SaveAsync()
    {
        return Context.SaveChangesAsync();
    }
}