using MediatR;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Queues;

public static class CalculateQueueDuration
{
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IWorkableQueueRepository _repository;

        public Handler(IWorkableQueueRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var queue = await _repository.GetAsync(request.QueueId);
            if (queue == null)
            {
                return new(Duration.Zero());
            }
            
            var duration = queue.CalculateDuration();

            return new(duration);
        }
    }

    public record Query(Guid QueueId) : IRequest<Response>;

    public record Response(Duration Duration);
}