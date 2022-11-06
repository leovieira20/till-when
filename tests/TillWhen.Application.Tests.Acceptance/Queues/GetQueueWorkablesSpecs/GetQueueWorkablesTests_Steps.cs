using FluentAssertions;
using LightBDD.Framework.Parameters;
using LightBDD.XUnit2;
using NSubstitute;
using TillWhen.Application.Queues;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Queues.GetQueueWorkablesSpecs;

public partial class GetQueueWorkablesTests : FeatureFixture
{
    private GetQueueWorkables.Response _response = null!;
    private readonly IWorkableQueueRepository _queueRepository;
    private readonly GetQueueWorkables.Handler _sut;

    public GetQueueWorkablesTests()
    {
        _queueRepository = Substitute.For<IWorkableQueueRepository>();
        _sut = new(_queueRepository);
    }

    private Task GivenAQueueWithWorkables(InputTable<IWorkable> workables)
    {
        _queueRepository
            .GetAsync(default)
            .ReturnsForAnyArgs(WorkableQueue.WithWorkables(workables.ToList()));
        
        return Task.CompletedTask;
    }

    private Task GivenAQueueSetUpFor8HWithWorkables(InputTable<IWorkable> workables)
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

    private Task ThenAListOfTasksForTodayIsReturned(InputTable<IWorkable> workables)
    {
        var today = _response.Days.First();

        today.Date.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
        today.WorkableSplits.Should().BeEquivalentTo(workables.ToList());
        
        return Task.CompletedTask;
    }

    private Task ThenAListOfQueueDaysIsReturned(InputTable<QueueDay> days)
    {
        var expectedFirstDay = days.First();
        var actualFirstDay = _response.Days.First();

        actualFirstDay.Date.Should().Be(expectedFirstDay.Date);
        actualFirstDay.WorkableSplits.Should().BeEquivalentTo(actualFirstDay.WorkableSplits);
        
        var expectedSecondDay = days[1];
        var actualSecondDay = _response.Days[1];
        
        actualSecondDay.Date.Should().Be(expectedSecondDay.Date);
        actualSecondDay.WorkableSplits.Should().BeEquivalentTo(expectedSecondDay.WorkableSplits);
        
        return Task.CompletedTask;
    }
}