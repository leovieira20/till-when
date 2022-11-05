using TillWhen.Domain.Common;

namespace TillWhen.Api.Endpoints.Workables.Common;

public record WorkableDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Duration Duration { get; set; }
}