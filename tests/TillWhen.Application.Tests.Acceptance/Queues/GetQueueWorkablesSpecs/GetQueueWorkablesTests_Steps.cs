using FluentAssertions;
using LightBDD.Framework.Parameters;
using LightBDD.XUnit2;
using NSubstitute;
using OneOf;
using TillWhen.Application.Queues;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Queues.GetQueueWorkablesSpecs;

public partial class GetQueueWorkablesTests : FeatureFixture
{
    private OneOf<List<QueueDay>> _response = null!;
    private readonly IWorkableQueueRepository _queueRepository;
    private readonly GetQueueWorkables.Handler _sut;

    public GetQueueWorkablesTests()
    {
        _queueRepository = Substitute.For<IWorkableQueueRepository>();
        _sut = new(_queueRepository);
    }

    private Task GivenAQueueWithWorkables(InputTable<WorkableBase> workables)
    {
        _queueRepository
            .GetAsync(default)
            .ReturnsForAnyArgs(WorkableQueue.WithWorkables(workables.ToList()));
        
        return Task.CompletedTask;
    }

    private Task GivenAQueueSetUpFor8HWithWorkables(InputTable<WorkableBase> workables)
    {
        var queue = WorkableQueue.WithCapacityAndWorkables("8h", workables.ToList());
        
        _queueRepository
            .GetAsync(default)
            .ReturnsForAnyArgs(queue);
        
        return Task.CompletedTask;
    }

    private async Task WhenHandlerIsExecuted()
    {
        _response = await _sut.Handle(new(), CancellationToken.None);
    }

    private Task ThenAListOfTasksForTodayIsReturned(InputTable<WorkableSplit> workables)
    {
        var today = _response.AsT0.First();

        today.Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        today.WorkableSplits.Should().BeEquivalentTo(workables.ToList());
        
        return Task.CompletedTask;
    }

    private Task ThenAListOfQueueDaysIsReturned(InputTable<QueueDay> days)
    {
        var expectedFirstDay = days.First();
        var actualFirstDay = _response.AsT0.First();

        actualFirstDay.Date.Should().BeCloseTo(expectedFirstDay.Date, TimeSpan.FromSeconds(1));
        actualFirstDay.WorkableSplits.Should().BeEquivalentTo(actualFirstDay.WorkableSplits);
        
        var expectedSecondDay = days[1];
        var actualSecondDay = _response.AsT0[1];
        
        actualSecondDay.Date.Should().BeCloseTo(expectedSecondDay.Date, TimeSpan.FromSeconds(1));
        actualSecondDay.WorkableSplits.Should().BeEquivalentTo(expectedSecondDay.WorkableSplits);
        
        return Task.CompletedTask;
    }
}