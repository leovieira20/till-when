using MediatR;
using OneOf;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Queues;

public static class CalculateQueueDuration
{
    public class Handler : IRequestHandler<Query, OneOf<Duration>>
    {
        private readonly IWorkableQueueRepository _repository;

        public Handler(IWorkableQueueRepository repository)
        {
            _repository = repository;
        }

        public async Task<OneOf<Duration>> Handle(Query request, CancellationToken cancellationToken)
        {
            var queue = await _repository.GetAsync(request.QueueId);
            if (queue == null)
            {
                return OneOf<Duration>.FromT0(Duration.Zero());
            }
            
            var duration = queue.CalculateDuration();

            return OneOf<Duration>.FromT0(duration);
        }
    }

    public record Query(Guid QueueId) : IRequest<OneOf<Duration>>;
}