using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using Xunit;
using ProjectEntity = TillWhen.Domain.Aggregates.ProjectAggregate.Project;

namespace TillWhen.Domain.Tests.Entities.Queues;

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
        var project = ProjectEntity.Create();
        project.AddTask(new ProjectTask(new (), TimeSpan.FromHours(1)));

        var projects = new List<ProjectEntity>
        {
            project
        };

        _queue = TaskQueue.WithProjects(projects);
    }

    private void GivenQueueWithOnProjectAndTwoTasksDueInDifferentDays()
    {
        var project = ProjectEntity.WithQuota(TimeSpan.FromHours(4));

        project.AddTask(new ProjectTask(new(), TimeSpan.FromHours(4)));
        project.AddTask(new ProjectTask(new(), TimeSpan.FromHours(4)));

        var projects = new List<ProjectEntity>
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

public class TaskQueue
{
    private TaskQueue()
    {
        Projects = new();
    }

    public static TaskQueue WithProjects(List<ProjectEntity> projects)
    {
        return new()
        {
            Projects = projects
        };
    }

    public QueueDay GetTasksForDate(DateTime date)
    {
        if (!Projects.Any())
        {
            return QueueDay.Default();
        }

        var tasks = Projects.SelectMany(x => x.GetTasksForDate(date));

        return QueueDay.WithTasks(tasks.ToList());
    }

    private List<ProjectEntity> Projects { get; init; }
}

public class QueueDay
{
    public static QueueDay Default()
    {
        return new();
    }

    private QueueDay()
    {
        Tasks = new();
    }

    public static QueueDay WithTasks(List<ProjectTask> tasks)
    {
        return new()
        {
            Tasks = tasks
        };
    }

    public List<ProjectTask> Tasks { get; private init; }
}