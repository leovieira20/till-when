using TillWhen.Domain.Common;

namespace TillWhen.Api.Endpoints.Projects.Common;

public record ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Duration Duration { get; set; }
}