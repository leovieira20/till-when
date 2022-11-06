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
            .Aggregate(Duration.Zero(), (current, p) => current + p.Estimation);
    }

    public List<QueueDay> GetWorkablesPerDay()
    {
        var workableSplits = Workables.SelectMany(x => x.GetSplitsFor(Capacity));
        
        var day = QueueDay.Empty(Capacity);
        var workablesPerDay = new List<QueueDay> { day };

        foreach (var w in workableSplits)
        {
            if (day.HasCapacityFor(w))
            {
                day.ScheduleWorkable(w);
            }
            else
            {
                day = day.NextDay();
                day.ScheduleWorkable(w);
                workablesPerDay.Add(day);
            }
        }

        return workablesPerDay;
    }

    public Guid Id { get; private set; }
    public Duration Capacity { get; private set; }
    public List<IWorkable> Workables { get; internal set; }
}