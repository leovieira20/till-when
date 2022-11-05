using MediatR;
using TillWhen.Domain.Aggregates.QueueAggregate;

namespace TillWhen.Application.Queues;

public static class GetQueueWorkables
{
    public record Request : IRequest<Response>;
    
    internal class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWorkableQueueRepository _queueRepository;

        public Handler(IWorkableQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var queue = await _queueRepository.GetAsync(Guid.Empty);

            return new(queue.GetWorkablesPerDay());
        }
    }

    public record Response(List<QueueDay> Days);
}