using FastEndpoints;
using MediatR;
using TillWhen.Application.Queues;

namespace TillWhen.Api.Endpoints.Queues.GetDuration;

public class GetQueueDurationEndpoint : Endpoint<GetQueueDurationRequest, GetQueueDurationResponse>
{
    private readonly IMediator _mediator;

    public GetQueueDurationEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/queue/{queueId}/duration");
    }
    
    public override async Task HandleAsync(GetQueueDurationRequest req, CancellationToken ct)
    {
        var response = await _mediator.Send(new CalculateQueueDuration.Query(req.QueueId), ct);

        await response.Match(async duration =>
        {
            await SendAsync(new(duration), cancellation: ct);
        });
    }
}