using System;
using System.Collections.Generic;
using System.Linq;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class WorkableQueue
{
    private WorkableQueue()
        : this("16h")
    {
    }

    private WorkableQueue(Duration capacity)
    {
        Capacity = capacity;
        Workables = new();
    }

    public static WorkableQueue Empty() => new();
    public static WorkableQueue WithWorkables(List<IWorkable> workables) => new() { Workables = workables };
    public static WorkableQueue WithCapacityAndWorkables(Duration capacity, List<IWorkable> workables) =>
        new(capacity) { Workables = workables };

    public Duration CalculateDuration()
    {
        if (!Workables.Any())
        {
            return Duration.Zero();
        }

        return Workables
            .Aggregate(Duration.Zero(), (current, p) => current + p.Duration);
    }

    public List<QueueDay> GetWorkablesPerDay()
    {
        var day = QueueDay.Empty(Capacity);
        var workablesPerDay = new List<QueueDay> { day };

        foreach (var w in Workables)
        {
            var currentWorkable = w;
            while (currentWorkable.HasRemainingEffort())
            {
                if (day.HasCapacityFor(w))
                {
                    currentWorkable = day.ScheduleWorkable(w);
                }
                else
                {
                    day = day.NextDay();
                    currentWorkable = day.ScheduleWorkable(currentWorkable);
                    workablesPerDay.Add(day);
                }                
            }
        }

        return workablesPerDay;
    }

    public Guid Id { get; private set; }
    public Duration Capacity { get; private set; }
    public List<IWorkable> Workables { get; internal set; }
}