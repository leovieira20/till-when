using FastEndpoints;
using TillWhen.Application.Projects;

namespace TillWhen.Api.Endpoints.Projects.Create;

public class CreateProjectCommandMapper : Mapper<CreateProjectRequest, CreateProjectResponse, CreateProject.Command>
{
    public override CreateProject.Command ToEntity(CreateProjectRequest r)
    {
        return new(r.Title, r.Duration);
    }
}