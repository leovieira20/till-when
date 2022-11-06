using System.Linq;
using FluentAssertions;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using Xunit;

namespace TillWhen.Domain.Tests.Unit.Aggregates.WorkableQueueAggregate;

public class WorkableQueueTests
{
    private readonly WorkableQueue _sut;

    public WorkableQueueTests()
    {
        _sut = WorkableQueue.Empty();
    }

    [Fact]
    public void ScheduleWorkableThatOverflowsADayIntoTwoDays()
    {
        _sut.Workables = new()
        {
            Workable.Create("Workable 01", "18h")
        };

        var days = _sut.GetWorkablesPerDay();

        var firstDay = days.First();
        firstDay.Workables.First().ScheduledEffort.Hours.Should().Be(16);

        var secondDay = days.Last();
        secondDay.Workables.First().ScheduledEffort.Hours.Should().Be(2);
    }
}