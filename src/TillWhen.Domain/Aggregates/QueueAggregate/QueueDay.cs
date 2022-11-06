using System;
using System.Collections.Generic;
using System.Linq;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class QueueDay
{
    private readonly Duration _capacity;

    public static QueueDay Empty() => new(DateTime.UtcNow);
    public static QueueDay Empty(Duration capacity) => new(DateTime.UtcNow, capacity);
    public static QueueDay WithWorkables(DateTime date, List<IWorkable> tasks) => new(date) { Workables = tasks };

    private QueueDay() { }
    internal QueueDay(DateTime date)
        : this(date, "16h") { }
    private QueueDay(DateTime date, Duration capacity)
    {
        Date = DateOnly.FromDateTime(date);
        _capacity = capacity;
    }
    
    public QueueDay NextDay() => new(Date.ToDateTime(TimeOnly.MinValue).AddDays(1), _capacity);

    public bool HasCapacityFor(IWorkable task) => _capacity - UsedCapacity > Duration.Zero();

    public void ScheduleWorkable(IWorkable task)
    {
        Workables.Add(task);
    }

    private Duration UsedCapacity =>
        Workables.Aggregate(Duration.Zero(), (duration, workable) => duration + workable.ScheduledEffort);

    public DateOnly Date { get; private set; }

    public List<IWorkable> Workables { get; internal init; } = new();
}