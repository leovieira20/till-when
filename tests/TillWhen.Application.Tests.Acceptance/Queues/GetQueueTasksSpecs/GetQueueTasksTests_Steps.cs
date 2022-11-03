using FluentAssertions;
using LightBDD.Framework.Parameters;
using LightBDD.XUnit2;
using NSubstitute;
using TillWhen.Application.Queues;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Queues.GetQueueTasksSpecs;

public partial class GetQueueProjectsTests : FeatureFixture
{
    private GetQueueProjects.Response _response = null!;
    private InputTable<IWorkable> _tasks = null!;
    private readonly ITaskQueueRepository _queueRepository;
    private readonly GetQueueProjects.Handler _sut;

    public GetQueueProjectsTests()
    {
        _queueRepository = Substitute.For<ITaskQueueRepository>();
        _sut = new(_queueRepository);
    }

    private Task GivenAQueueWithTasks(InputTable<IWorkable> projects)
    {
        _tasks = projects;
        _queueRepository
            .GetAsync(default)
            .ReturnsForAnyArgs(TaskQueue.WithTasks(_tasks.ToList()));
        
        return Task.CompletedTask;
    }

    private async Task WhenHandlerIsExecuted()
    {
        _response = await _sut.Handle(new(), CancellationToken.None);
    }

    private Task ThenAListOfTasksForTodayIsReturned()
    {
        var today = _response.Days.First();

        today.Date.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
        today.Tasks.Should().BeEquivalentTo(_tasks);
        
        return Task.CompletedTask;
    }
}