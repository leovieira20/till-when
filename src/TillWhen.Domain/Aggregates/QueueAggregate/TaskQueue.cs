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
        var day = QueueDay.Default();
        var tasksPerDay = new List<QueueDay> { day};

        foreach (var t in Tasks)
        {
            if (day.HasCapacityFor(t))
            {
                day.AddTask(t);
            }
            else
            {
                day = new(day.Date.AddDays(1));
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

    public Guid Id { get; set; }
    public List<IWorkable> Tasks { get; private set; }
}