using MediatR;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Workables;

public static class CreateWorkable
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly IWorkableRepository _repository;

        public Handler(IWorkableRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            var workable = Workable.Create(command.Title, command.Duration);
            
            _repository.Add(workable);
            await _repository.CommitAsync();

            return new (workable.Id, workable.Duration);
        }
    }

    public record Command(string Title, Duration Duration) : IRequest<Response>;

    public record Response(Guid Id, Duration Duration);
}