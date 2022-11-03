using System.Collections.Generic;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class QueueDay
{
    public static QueueDay Default()
    {
        return new();
    }

    private QueueDay()
    {
        Tasks = new();
    }

    public static QueueDay WithTasks(List<IWorkable> tasks)
    {
        return new()
        {
            Tasks = tasks
        };
    }

    public List<IWorkable> Tasks { get; private init; }
}