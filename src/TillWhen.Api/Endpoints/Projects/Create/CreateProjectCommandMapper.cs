using FastEndpoints;
using TillWhen.Application.Projects;
using TillWhen.Domain.Common;

namespace TillWhen.Api.Endpoints.Projects.Create;

public class CreateProjectCommandMapper : Mapper<CreateProjectRequest, CreateProjectResponse, CreateProject.Command>
{
    public override CreateProject.Command ToEntity(CreateProjectRequest r)
    {
        return new()
        {
            Title = r.Title,
            Duration = new Duration(r.Duration)
        };
    }
}