using MediatR;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Common;

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
            var project = Project.Create(command.Title, command.Duration);
            
            _repository.Add(project);
            await _repository.CommitAsync();

            return new (project.Id, project.Duration);
        }
    }

    public record Command(string Title, Duration Duration) : IRequest<Response>;

    public record Response(Guid Id, Duration Duration);
}