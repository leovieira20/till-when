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
    public static QueueDay WithTasks(DateTime date, List<IWorkable> tasks) => new(date) { Tasks = tasks };

    private QueueDay() { }
    internal QueueDay(DateTime date)
        : this(date, "16h") { }
    private QueueDay(DateTime date, Duration capacity)
    {
        Date = DateOnly.FromDateTime(date);
        _capacity = capacity;
    }
    
    public QueueDay NextDay() => new(Date.ToDateTime(TimeOnly.MinValue).AddDays(1), _capacity);

    public bool HasCapacityFor(IWorkable task) => UsedCapacity + task.Duration <= _capacity;

    public void ScheduleTask(IWorkable task)
    {
        task.ReduceEffortBy(_capacity - UsedCapacity);
        Tasks.Add(task);
    }

    public DateOnly Date { get; private set; }

    private Duration UsedCapacity =>
        Tasks.Aggregate(Duration.Empty(), (duration, workable) => duration + workable.Duration);

    public List<IWorkable> Tasks { get; internal init; } = new();
}