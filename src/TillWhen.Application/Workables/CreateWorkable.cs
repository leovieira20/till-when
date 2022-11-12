using MediatR;
using OneOf;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Workables;

public static class CreateWorkable
{
    public class Handler : IRequestHandler<Command, OneOf<(Guid, Duration)>>
    {
        private readonly IWorkableRepository _repository;

        public Handler(IWorkableRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<OneOf<(Guid, Duration)>> Handle(Command command, CancellationToken cancellationToken)
        {
            var workable = Workable.Create(command.Title, command.Duration);
            
            _repository.Create(workable);
            await _repository.SaveAsync();

            return OneOf<(Guid, Duration)>.FromT0((workable.Id, workable.Estimation));
        }
    }

    public record Command(string Title, Duration Duration) : IRequest<OneOf<(Guid, Duration)>>;
}