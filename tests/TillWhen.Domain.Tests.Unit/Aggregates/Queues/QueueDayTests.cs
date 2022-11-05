using System;
using FluentAssertions;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;
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
    public void CanIncludeOneMoreTask()
    {
        var existingTask = Project.Create(Guid.NewGuid().ToString(), "1h");

        _queueDay.ScheduleTask(existingTask);

        var extraTask = Project.Create(Guid.NewGuid().ToString(), "1h");
        
        _queueDay.HasCapacityFor(extraTask).Should().BeTrue();
    }
    
    [Fact]
    public void CannotIncludeOneMoreTask()
    {
        var existingTask = Project.Create(Guid.NewGuid().ToString(), "24h");
        
        _queueDay.ScheduleTask(existingTask);

        var extraTask = Project.Create(Guid.NewGuid().ToString(), "1h");
        
        _queueDay.HasCapacityFor(extraTask).Should().BeFalse();
    }

    [Fact]
    public void ReducesTasksRemainingEffort()
    {
        var task = Project.Create(Guid.NewGuid().ToString(), "1h");

        _queueDay.ScheduleTask(task);
        
        task.ScheduledEffort.Hours.Should().Be(1);
        task.RemainingEffort.Hours.Should().Be(0);
    }
}