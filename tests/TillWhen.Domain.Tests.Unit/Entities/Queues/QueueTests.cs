using System;
using System.Collections.Generic;
using FluentAssertions;
using TestStack.BDDfy;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;
using Xunit;

namespace TillWhen.Domain.Tests.Unit.Entities.Queues;

public class QueueTests
{
    private TaskQueue _queue;
    private QueueDay _daySlot;

    [Fact]
    public void EmptyQueueReturnDefaultDailyView()
    {
        this.Given(x => x.GivenEmptyQueue())
            .When(x => x.WhenIAccessTasksForDay(DateTime.UtcNow))
            .Then(x => x.ThenEmptyDaySlotIsReturned())
            .BDDfy();
    }

    [Fact]
    public void QueueWithOneProjectWithOnePendingTask()
    {
        this.Given(x => x.GivenQueueWithOnProjectAndOnePendingTask())
            .When(x => x.WhenIAccessTasksForDay(DateTime.UtcNow))
            .Then(x => x.ThenDaySlotWithOnTaskIsReturned())
            .BDDfy();
    }

    [Fact]
    public void QueueWithOneProjectWithTwoPendingTasksDueInDifferentDays()
    {
        this.Given(x => x.GivenQueueWithOnProjectAndTwoTasksDueInDifferentDays())
            .When(x => x.WhenIAccessTasksForDay(DateTime.UtcNow))
            .Then(x => x.ThenDaySlotWithOnTaskIsReturned())
            .When(x => x.WhenIAccessTasksForDay(DateTime.UtcNow.AddDays(1)))
            .Then(x => x.ThenDaySlotWithOnTaskIsReturned())
            .BDDfy();
    }

    private void GivenEmptyQueue()
    {
        _queue = TaskQueue.WithProjects(new());
    }

    private void GivenQueueWithOnProjectAndOnePendingTask()
    {
        var project = Project.Create();
        project.AddTask(new(new (), TimeSpan.FromHours(1)));

        var projects = new List<Project>
        {
            project
        };

        _queue = TaskQueue.WithProjects(projects);
    }

    private void GivenQueueWithOnProjectAndTwoTasksDueInDifferentDays()
    {
        var project = Project.WithQuota(TimeSpan.FromHours(4));

        project.AddTask(new(new(), TimeSpan.FromHours(4)));
        project.AddTask(new(new(), TimeSpan.FromHours(4)));

        var projects = new List<Project>
        {
            project
        };

        _queue = TaskQueue.WithProjects(projects);
    }

    private void WhenIAccessTasksForDay(DateTime date)
    {
        _daySlot = _queue.GetTasksForDate(date);
    }

    private void ThenDaySlotWithOnTaskIsReturned()
    {
        _daySlot.Tasks.Count.Should().Be(1);
    }

    private void ThenEmptyDaySlotIsReturned()
    {
        _daySlot.Tasks.Should().BeEmpty();
    }
}