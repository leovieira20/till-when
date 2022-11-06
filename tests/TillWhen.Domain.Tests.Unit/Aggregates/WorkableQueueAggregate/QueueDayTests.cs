using System;
using FluentAssertions;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using Xunit;

namespace TillWhen.Domain.Tests.Unit.Aggregates.WorkableQueueAggregate;

public class QueueDayTests
{
    private readonly QueueDay _queueDay;

    public QueueDayTests()
    {
        _queueDay = QueueDay.Empty();
    }

    [Fact]
    public void CanIncludeOneMoreWorkable()
    {
        var existingSplit = new WorkableSplit(Guid.NewGuid().ToString(), "1h");

        _queueDay.ScheduleWorkable(existingSplit);

        var extraSplit = new WorkableSplit(Guid.NewGuid().ToString(), "1h");
        
        _queueDay.HasCapacityFor(extraSplit).Should().BeTrue();
    }
    
    [Fact(Skip = "Skipping for now")]
    public void CannotIncludeOneMoreWorkable()
    {
        var existingSplit = new WorkableSplit(Guid.NewGuid().ToString(), "16h");
        
        _queueDay.ScheduleWorkable(existingSplit);

        var extraSplit = new WorkableSplit(Guid.NewGuid().ToString(), "1h");
        
        _queueDay.HasCapacityFor(extraSplit).Should().BeFalse();
    }

    [Fact(Skip = "Skipping for now")]
    public void ReducesWorkablesRemainingEffort()
    {
        var split = new WorkableSplit(Guid.NewGuid().ToString(), "1h");

        _queueDay.ScheduleWorkable(split);
        
        split.Estimation.Hours.Should().Be(1);
    }
}