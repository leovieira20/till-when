using FastEndpoints;
using MediatR;
using TillWhen.Application.Queues;

namespace TillWhen.Api.Endpoints.Queues.GetQueue;

public class GetQueueEndpoint : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public GetQueueEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Get("api/queue");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _mediator.Send(new GetQueueWorkables.Request(), ct);

        await response.Match(
            workables => SendOkAsync(workables, cancellation: ct)
        );
    }
}