using FluentAssertions;
using LightBDD.Framework.Parameters;
using LightBDD.XUnit2;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using OneOf;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Queues.CalculateQueueDurationSpecs;

public partial class CalculateQueueDurationTests : FeatureFixture
{
    private readonly Application.Queues.CalculateQueueDuration.Handler _sut;
    private OneOf<Duration> _result = null!;
    private readonly IWorkableQueueRepository _queueRepository;

    public CalculateQueueDurationTests()
    {
        _queueRepository = Substitute.For<IWorkableQueueRepository>();
        _sut = new(_queueRepository);
    }
    
    private Task GivenAnInexistentQueue()
    {
        _queueRepository
            .GetAsync(Arg.Any<Guid>())
            .ReturnsNullForAnyArgs();

        return Task.CompletedTask;
    }
    
    private Task GivenAQueueWithNoWorkables()
    {
        _queueRepository
            .GetAsync(Arg.Any<Guid>())
            .ReturnsForAnyArgs(WorkableQueue.Empty());
        
        return Task.CompletedTask;
    }

    private Task GivenAQueueWithWorkables(InputTable<IWorkable> projects)
    {
        _queueRepository
            .GetAsync(Arg.Any<Guid>())
            .ReturnsForAnyArgs(WorkableQueue.WithWorkables(projects.ToList()));
        
        return Task.CompletedTask;
    }

    private async Task WhenTheActionIsExecuted()
    {
        _result = await _sut.Handle(new(Guid.NewGuid()), CancellationToken.None);
    }

    private Task TheDurationShouldBe(Duration duration)
    {
        _result.AsT0.Days.Should().Be(duration.Days);
        _result.AsT0.Hours.Should().Be(duration.Hours);
        _result.AsT0.Minutes.Should().Be(duration.Minutes);
        
        return Task.CompletedTask;
    }
}