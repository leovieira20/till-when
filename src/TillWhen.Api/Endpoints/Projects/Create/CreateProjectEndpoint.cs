using FastEndpoints;
using MediatR;
using TillWhen.Api.Endpoints.Projects.GetByID;

namespace TillWhen.Api.Endpoints.Projects.Create;

public class CreateProjectEndpoint : Endpoint<CreateProjectRequest, CreateProjectResponse, CreateProjectCommandMapper>
{
    private readonly IMediator _mediator;

    public CreateProjectEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/projects");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateProjectRequest req, CancellationToken ct)
    {
        var response = await _mediator.Send(Map.ToEntity(req), ct);

        await SendCreatedAtAsync(
            GetProjectByIdEndpoint.Name,
            new { response.Id },
            new(response.Id),
            cancellation: ct);
    }
}