using MediatR;
using TillWhen.Domain.Aggregates.ProjectAggregate;

namespace TillWhen.Application.Projects;

public static class CreateProject
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly IProjectRepository _repository;

        public Handler(IProjectRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            var project = Project.Create();
            
            _repository.Add(project);
            await _repository.CommitAsync();

            return new (project.Id);
        }
    }

    public record Command : IRequest<Response>;

    public record Response(Guid Id);
}