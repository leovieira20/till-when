using System;
using System.Collections.Generic;
using System.Linq;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class QueueDay
{
    private readonly Duration _capacity;

    public static QueueDay Empty() => new(DateTime.UtcNow);
    public static QueueDay Empty(Duration capacity) => new(DateTime.UtcNow, capacity);
    public static QueueDay WithWorkables(DateTime date, List<IWorkableSplit> splits) => new(date) { WorkableSplits = splits };

    private QueueDay() { }
    internal QueueDay(DateTime date)
        : this(date, "16h") { }
    private QueueDay(DateTime date, Duration capacity)
    {
        Date = DateOnly.FromDateTime(date);
        _capacity = capacity;
    }
    
    public QueueDay NextDay() => new(Date.ToDateTime(TimeOnly.MinValue).AddDays(1), _capacity);

    public bool HasCapacityFor(IWorkableSplit split) => _capacity - UsedCapacity > Duration.Zero();

    public void ScheduleWorkable(IWorkableSplit split)
    {
        WorkableSplits.Add(split);
    }

    private Duration UsedCapacity =>
        WorkableSplits.Aggregate(Duration.Zero(), (duration, workable) => duration + workable.Estimation);

    public DateOnly Date { get; private set; }

    public List<IWorkableSplit> WorkableSplits { get; internal init; } = new();
}