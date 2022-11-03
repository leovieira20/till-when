using System;
using FluentAssertions;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;
using Xunit;

namespace TillWhen.Domain.Tests.Unit.Aggregates.Queues;

public class QueueDayTests
{
    [Fact]
    public void CanIncludeOneMoreTask()
    {
        var existingTask = Project.Create(Guid.NewGuid().ToString(), Duration.Create("1h"));
        
        var queueDay = new QueueDay(DateTime.UtcNow) { Tasks = new() { existingTask } };

        var extraTask = Project.Create(Guid.NewGuid().ToString(), Duration.Create("1h"));
        
        queueDay.HasCapacityFor(extraTask).Should().BeTrue();
    }
    
    [Fact]
    public void CannotIncludeOneMoreTask()
    {
        var existingTask = Project.Create(Guid.NewGuid().ToString(), Duration.Create("24h"));
        
        var queueDay = new QueueDay(DateTime.UtcNow) { Tasks = new() { existingTask } };

        var extraTask = Project.Create(Guid.NewGuid().ToString(), Duration.Create("1h"));
        
        queueDay.HasCapacityFor(extraTask).Should().BeFalse();
    }

    [Fact]
    public void ReducesTasksRemainingEffort()
    {
        var task = Project.Create(Guid.NewGuid().ToString(), Duration.Create("1h"));
        
        var queueDay = QueueDay.Empty();
        
        queueDay.ScheduleTask(task);
        
        task.RemainingEffort.Should().Be(Duration.Create("0h"));
    }
}