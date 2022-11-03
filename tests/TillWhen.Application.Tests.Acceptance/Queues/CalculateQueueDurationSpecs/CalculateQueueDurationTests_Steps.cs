using FluentAssertions;
using LightBDD.Framework.Parameters;
using LightBDD.XUnit2;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Queues.CalculateQueueDurationSpecs;

public partial class CalculateQueueDurationTests : FeatureFixture
{
    private readonly Application.Queues.CalculateQueueDuration.Handler _sut;
    private Application.Queues.CalculateQueueDuration.Response _result = null!;
    private readonly ITaskQueueRepository _queueRepository;

    public CalculateQueueDurationTests()
    {
        _queueRepository = Substitute.For<ITaskQueueRepository>();
        _sut = new(_queueRepository);
    }
    
    private void GivenAnInexistentQueue()
    {
        _queueRepository
            .GetAsync(Arg.Any<Guid>())
            .ReturnsNullForAnyArgs();

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
        _result.Duration.Days.Should().Be(duration.Days);
        _result.Duration.Hours.Should().Be(duration.Hours);
        _result.Duration.Minutes.Should().Be(duration.Minutes);
    }
}