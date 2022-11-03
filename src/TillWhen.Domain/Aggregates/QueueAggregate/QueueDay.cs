using System;
using System.Collections.Generic;
using System.Linq;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class QueueDay
{
    private readonly Duration _capacity = Duration.Create("24h");

    public static QueueDay Default() => new(DateTime.UtcNow);
    public static QueueDay WithTasks(DateTime date, List<IWorkable> tasks) => new(date) { Tasks = tasks };

    private QueueDay()
    {
    }

    internal QueueDay(DateTime date)
        : this(DateOnly.FromDateTime(date))
    {
    }
    internal QueueDay(DateOnly date)
    {
        Date = date;
        Tasks = new();
    }

    public bool HasCapacityFor(IWorkable task)
    {
        return UsedCapacity + task.Duration <= _capacity;
    }

    public void AddTask(IWorkable task)
    {
        Tasks.Add(task);
    }

    public DateOnly Date { get; private set; }

    private Duration UsedCapacity =>
        Tasks.Aggregate(Duration.Empty(), (duration, workable) => duration + workable.Duration);

    public List<IWorkable> Tasks { get; internal init; } = new();
}