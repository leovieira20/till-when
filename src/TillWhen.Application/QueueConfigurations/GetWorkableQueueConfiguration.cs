using MediatR;
using OneOf;
using TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;

namespace TillWhen.Application.QueueConfigurations;

public static class GetWorkableQueueConfiguration
{
    public class Handler : IRequestHandler<Request, OneOf<WorkableQueueConfiguration>>
    {
        private readonly IWorkableQueueConfigurationRepository _repository;

        public Handler(IWorkableQueueConfigurationRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<OneOf<WorkableQueueConfiguration>> Handle(Request request, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetAsync(request.WorkableQueueId);

            return configuration;
        }
    }

    public record Request(Guid WorkableQueueId) : IRequest<OneOf<WorkableQueueConfiguration>>;
}