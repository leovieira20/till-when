using MediatR;
using OneOf;
using TillWhen.Domain.Aggregates.QueueAggregate;

namespace TillWhen.Application.Queues;

public static class GetQueueWorkables
{
    internal class Handler : IRequestHandler<Request, OneOf<List<QueueDay>>>
    {
        private readonly IWorkableQueueRepository _queueRepository;

        public Handler(IWorkableQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        public async Task<OneOf<List<QueueDay>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var queue = await _queueRepository.GetAsync(Guid.Empty);

            return OneOf<List<QueueDay>>.FromT0(queue.GetWorkablesPerDay());
        }
    }

    public record Request : IRequest<OneOf<List<QueueDay>>>;
}