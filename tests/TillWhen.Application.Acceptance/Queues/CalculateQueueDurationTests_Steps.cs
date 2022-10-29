using FluentAssertions;
using LightBDD.Framework.Parameters;
using LightBDD.XUnit2;
using NSubstitute;
using TillWhen.Application.Queues;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Acceptance.Queues;

public partial class CalculateQueueDurationTests : FeatureFixture
{
    private readonly CalculateQueueDuration.Handler _sut;
    private CalculateQueueDuration.Response _result = null!;
    private readonly ITaskQueueRepository _queueRepository;

    public CalculateQueueDurationTests()
    {
        _queueRepository = Substitute.For<ITaskQueueRepository>();
        _sut = new(_queueRepository);
    }
    
    private void GivenAQueueWithNoProjects()
    {
        _queueRepository
            .GetAsync(Arg.Any<Guid>())
            .ReturnsForAnyArgs(TaskQueue.Empty());
    }

    private void GivenAQueueWithProjects(InputTable<Project> projects)
    {
        _queueRepository
            .GetAsync(Arg.Any<Guid>())
            .ReturnsForAnyArgs(TaskQueue.WithProjects(projects.ToList()));
    }

    private async Task WhenTheActionIsExecuted()
    {
        _result = await _sut.Handle(new(Guid.NewGuid()), CancellationToken.None);
    }

    private void TheDurationShouldBe(Duration duration)
    {
        _result.Duration.Should().BeEquivalentTo(new
        {
            duration.Days,
            duration.Hours,
            duration.Minutes
        });
    }
}