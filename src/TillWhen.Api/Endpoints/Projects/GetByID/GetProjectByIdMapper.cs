using FastEndpoints;
using TillWhen.Api.Endpoints.Projects.Common;
using TillWhen.Domain.Aggregates.ProjectAggregate;

namespace TillWhen.Api.Endpoints.Projects.GetByID;

public class GetProjectByIdMapper : IMapper
{
    public ProjectDto FromEntity(Project project)
    {
        return new()
        {
            Id = project.Id
        };
    }
}