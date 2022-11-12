using FastEndpoints;
using MediatR;
using TillWhen.Application.QueueConfigurations;

namespace TillWhen.Api.Endpoints.QueueConfigurations;

public class UpdateQueueConfigurationCapacityEndpoint : Endpoint<UpdateQueueConfigurationCapacityRequest>
{
    private readonly IMediator _mediator;

    public UpdateQueueConfigurationCapacityEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Patch("api/queueConfiguration/{id}/updateCapacity");
    }

    public override async Task HandleAsync(UpdateQueueConfigurationCapacityRequest req, CancellationToken ct)
    {
        var response = await _mediator.Send(new UpdateQueueConfigurationCapacity.Command(req.Id, req.Hours), ct);

        await response.Match(
            success => SendOkAsync(cancellation: ct));
    }
}

public class UpdateQueueConfigurationCapacityRequest
{
    public Guid Id { get; set; }
    public int Hours { get; set; }
}