using MediatR;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;

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

            return new(queue.Projects);
        }
    }

    public record Response(List<Project> Projects);
}