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
        var existingWorkable = Workable.Create(Guid.NewGuid().ToString(), "1h");

        _queueDay.ScheduleWorkable(existingWorkable);

        var extraWorkable = Workable.Create(Guid.NewGuid().ToString(), "1h");
        
        _queueDay.HasCapacityFor(extraWorkable).Should().BeTrue();
    }
    
    [Fact(Skip = "Skipping for now")]
    public void CannotIncludeOneMoreWorkable()
    {
        var existingWorkable = Workable.Create(Guid.NewGuid().ToString(), "16h");
        
        _queueDay.ScheduleWorkable(existingWorkable);

        var extraWorkable = Workable.Create(Guid.NewGuid().ToString(), "1h");
        
        _queueDay.HasCapacityFor(extraWorkable).Should().BeFalse();
    }

    [Fact(Skip = "Skipping for now")]
    public void ReducesWorkablesRemainingEffort()
    {
        var workable = Workable.Create(Guid.NewGuid().ToString(), "1h");

        _queueDay.ScheduleWorkable(workable);
        
        workable.ScheduledEffort.Hours.Should().Be(1);
        workable.RemainingEffort.Hours.Should().Be(0);
    }
}