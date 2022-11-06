using FastEndpoints;
using MediatR;
using TillWhen.Api.Endpoints.Workables.GetByID;

namespace TillWhen.Api.Endpoints.Workables.Create;

public class CreateWorkableEndpoint : Endpoint<CreateWorkableRequest, CreateWorkableResponse, CreateProjectCommandMapper>
{
    private readonly IMediator _mediator;

    public CreateWorkableEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/workable");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateWorkableRequest req, CancellationToken ct)
    {
        var response = await _mediator.Send(Map.ToEntity(req), ct);

        await response.Match(async tuple =>
        {
            await SendCreatedAtAsync(
                GetWorkableByIdEndpoint.Name,
                new { tuple.Item1 },
                new(tuple.Item1),
                cancellation: ct);
        });
    }
}