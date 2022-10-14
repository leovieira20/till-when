using TillWhen.Api.Endpoints.Projects.Common;

namespace TillWhen.Api.Endpoints.Projects.GetByID;

public class GetProjectByIdResponse
{
    public GetProjectByIdResponse(ProjectDto project)
    {
        Project = project;
    }

    public ProjectDto Project { get; set; }
}