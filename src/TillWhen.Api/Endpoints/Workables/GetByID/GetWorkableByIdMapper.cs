using FastEndpoints;
using TillWhen.Api.Endpoints.Workables.Common;
using TillWhen.Domain.Aggregates.WorkableAggregate;

namespace TillWhen.Api.Endpoints.Workables.GetByID;

public class GetWorkableByIdMapper : IMapper
{
    public WorkableDto FromEntity(Workable workable)
    {
        return new()
        {
            Id = workable.Id,
            Title = workable.Title,
            Duration = workable.Estimation,
        };
    }
}