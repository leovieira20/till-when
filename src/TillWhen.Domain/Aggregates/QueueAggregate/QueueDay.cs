using System;
using System.Collections.Generic;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class QueueDay
{
    public static QueueDay Default()
    {
        return new(DateTime.UtcNow);
    }

    public static QueueDay WithTasks(DateTime date, List<IWorkable> tasks)
    {
        return new(date)
        {
            Tasks = tasks
        };
    }

    private QueueDay(DateTime date)
    {
        Date = DateOnly.FromDateTime(date);
        Tasks = new();
    }

    public DateOnly Date { get; private set; }

    public List<IWorkable> Tasks { get; private init; }
}