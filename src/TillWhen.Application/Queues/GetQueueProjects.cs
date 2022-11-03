using MediatR;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Queues;

public static class GetQueueProjects
{
    public record Request : IRequest<Response>;
    
    internal class Handler : IRequestHandler<Request, Response>
    {
        private readonly ITaskQueueRepository _queueRepository;

        public Handler(ITaskQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var queue = await _queueRepository.GetAsync(Guid.Empty);

            return new(queue.Tasks);
        }
    }

    public record Response(List<IWorkable> Projects);
}