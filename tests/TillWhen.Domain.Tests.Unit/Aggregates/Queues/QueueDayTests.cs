using System;
using FluentAssertions;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using Xunit;

namespace TillWhen.Domain.Tests.Unit.Aggregates.Queues;

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
    
    [Fact]
    public void CannotIncludeOneMoreWorkable()
    {
        var existingWorkable = Workable.Create(Guid.NewGuid().ToString(), "24h");
        
        _queueDay.ScheduleWorkable(existingWorkable);

        var extraWorkable = Workable.Create(Guid.NewGuid().ToString(), "1h");
        
        _queueDay.HasCapacityFor(extraWorkable).Should().BeFalse();
    }

    [Fact]
    public void ReducesWorkablesRemainingEffort()
    {
        var workable = Workable.Create(Guid.NewGuid().ToString(), "1h");

        _queueDay.ScheduleWorkable(workable);
        
        workable.ScheduledEffort.Hours.Should().Be(1);
        workable.RemainingEffort.Hours.Should().Be(0);
    }
}