using MediatR;
using OneOf;
using OneOf.Types;
using TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;

namespace TillWhen.Application.QueueConfigurations;

public static class UpdateQueueConfigurationCapacity
{
    public class Handler : IRequestHandler<Command, OneOf<Success>>
    {
        private readonly IWorkableQueueConfigurationRepository _repository;

        public Handler(IWorkableQueueConfigurationRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<OneOf<Success>> Handle(Command request, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetByIdAsync(request.Id);
            
            configuration.UpdateCapacity(request.Hours);
            
            _repository.Update(configuration);
            await _repository.SaveAsync();

            return new Success();
        }
    }

    public record Command(Guid Id, int Hours) : IRequest<OneOf<Success>>;
}