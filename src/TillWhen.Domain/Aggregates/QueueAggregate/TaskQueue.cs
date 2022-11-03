using System;
using System.Collections.Generic;
using System.Linq;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class TaskQueue
{
    private TaskQueue()
        : this(Duration.Create("16h"))
    {
    }

    private TaskQueue(Duration capacity)
    {
        Capacity = capacity;
        Tasks = new();
    }

    public static TaskQueue Empty() => new();
    public static TaskQueue WithTasks(List<IWorkable> tasks) => new() { Tasks = tasks };
    public static TaskQueue WithCapacityAndTasks(Duration capacity, List<IWorkable> tasks) =>
        new(capacity) { Tasks = tasks };

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
        var day = QueueDay.Default(Capacity);
        var tasksPerDay = new List<QueueDay> { day };

        foreach (var t in Tasks)
        {
            if (day.HasCapacityFor(t))
            {
                day.AddTask(t);
            }
            else
            {
                day = day.NextDay();
                day.AddTask(t);
                tasksPerDay.Add(day);
            }
        }

        return tasksPerDay;
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


    public Guid Id { get; private set; }
    public Duration Capacity { get; private set; }
    public List<IWorkable> Tasks { get; private set; }
}