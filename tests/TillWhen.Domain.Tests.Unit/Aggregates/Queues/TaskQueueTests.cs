using System.Linq;
using FluentAssertions;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;
using Xunit;

namespace TillWhen.Domain.Tests.Unit.Aggregates.Queues;

public class TaskQueueTests
{
    private readonly TaskQueue _sut;

    public TaskQueueTests()
    {
        _sut = TaskQueue.Empty();
    }

    [Fact]
    public void ScheduleTaskThatOverflowsADayIntoTwoDays()
    {
        _sut.Tasks = new()
        {
            Project.Create("Project 01", "18h")
        };

        var days = _sut.GetTasksPerDay();

        var firstDay = days.First();
        firstDay.Tasks.First().ScheduledDuration.Hours.Should().Be(16);

        var secondDay = days.Last();
        secondDay.Tasks.First().ScheduledDuration.Hours.Should().Be(2);
    }
}