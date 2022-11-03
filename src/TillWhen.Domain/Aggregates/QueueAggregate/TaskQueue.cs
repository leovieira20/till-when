using System;
using System.Collections.Generic;
using System.Linq;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class TaskQueue
{
    private TaskQueue()
        : this("16h")
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
        var day = QueueDay.Empty(Capacity);
        var tasksPerDay = new List<QueueDay> { day };

        foreach (var t in Tasks)
        {
            if (day.HasCapacityFor(t))
            {
                day.ScheduleTask(t);
            }
            else
            {
                day = day.NextDay();
                day.ScheduleTask(t);
                tasksPerDay.Add(day);
            }
        }

        return tasksPerDay;
    }

    public Guid Id { get; private set; }
    public Duration Capacity { get; private set; }
    public List<IWorkable> Tasks { get; private set; }
}