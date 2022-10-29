using System.Collections.Generic;
using TillWhen.Domain.Aggregates.ProjectAggregate;

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

    public static QueueDay WithTasks(List<ProjectTask> tasks)
    {
        return new()
        {
            Tasks = tasks
        };
    }

    public List<ProjectTask> Tasks { get; private init; }
}