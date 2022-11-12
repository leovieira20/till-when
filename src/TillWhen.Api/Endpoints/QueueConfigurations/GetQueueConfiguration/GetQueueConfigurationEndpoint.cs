using FastEndpoints;
using MediatR;
using TillWhen.Application.QueueConfigurations;

namespace TillWhen.Api.Endpoints.QueueConfigurations.GetQueueConfiguration;

public class GetQueueConfigurationEndpoint : Endpoint<GetQueueConfigurationRequest>
{
    private readonly IMediator _mediator;

    public GetQueueConfigurationEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Get("api/queueConfiguration");
    }

    public override async Task HandleAsync(GetQueueConfigurationRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetWorkableQueueConfiguration.Request(request.WorkableQueueId), 
            cancellationToken);

        await response.Match(
            configuration => SendOkAsync(configuration, cancellation: cancellationToken));
    }
}

public class GetQueueConfigurationRequest
{
    public Guid WorkableQueueId { get; set; } = Guid.NewGuid();
}