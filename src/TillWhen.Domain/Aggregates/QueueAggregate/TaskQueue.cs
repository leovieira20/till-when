using System;
using System.Collections.Generic;
using System.Linq;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class TaskQueue
{
    private TaskQueue()
    {
        Tasks = new();
    }

    public static TaskQueue Empty()
    {
        return new();
    }

    public static TaskQueue WithTasks(List<IWorkable> tasks)
    {
        return new()
        {
            Tasks = tasks
        };
    }

    public Duration CalculateDuration()
    {
        if (!Tasks.Any())
        {
            return Duration.Empty();
        }

        return Tasks
            .Aggregate(Duration.Empty(), (current, p) => current + p.Duration);
    }

    public List<QueueDay> GetTasksPerDay()
    {
        var day = QueueDay.WithTasks(DateTime.UtcNow, Tasks);
        return new() { day };
    }

    public QueueDay GetTasksForDate(DateTime date)
    {
        if (!Tasks.Any())
        {
            return QueueDay.Default();
        }

        var tasks = Tasks.SelectMany(x => x.GetTasksForDate(date));

        return QueueDay.WithTasks(DateTime.UtcNow, tasks.ToList());
    }

    public Guid Id { get; set; }
    public List<IWorkable> Tasks { get; private set; }
}