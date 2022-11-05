using System;
using System.Collections.Generic;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.SimpleTaskAggregate;

public class SimpleTask : IWorkable
{
    public double GetEstimate()
    {
        throw new NotImplementedException();
    }

    public string Status { get; set; }
    public Duration Duration { get; }
    public Duration ScheduledDuration { get; }

    public IEnumerable<IWorkable> GetTasksForDate(DateTime date)
    {
        throw new NotImplementedException();
    }

    public Project ScheduleEffortBy(Duration scheduledEffort)
    {
        throw new NotImplementedException();
    }

    public bool HasRemainingEffort()
    {
        throw new NotImplementedException();
    }
}