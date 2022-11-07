using System;
using System.Collections.Generic;
using System.Linq;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class WorkableQueue
{
    public static WorkableQueue Empty() => new();

    public static WorkableQueue WithWorkables(List<WorkableBase> workables)
    {
        var queue = new WorkableQueue
        {
            Workables = workables
        };

        foreach (var w in workables)
        {
            w.WorkableQueueId = queue.Id;
        }

        return queue;
    }

    public static WorkableQueue WithCapacityAndWorkables(Duration capacity, List<WorkableBase> workables) =>
        new(capacity) { Workables = workables };
    
    private WorkableQueue() => Id = Guid.NewGuid();
    private WorkableQueue(Duration capacity) : this() => Capacity = capacity;

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
    public Duration Capacity { get; private set; } = "16h";
    public List<WorkableBase> Workables { get; internal set; } = new();
}