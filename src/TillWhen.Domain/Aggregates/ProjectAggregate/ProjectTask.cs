using System;
using System.Collections.Generic;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.ProjectAggregate;

public record ProjectTask : IWorkable
{
    public double GetEstimate()
    {
        return Estimate.TotalMinutes;
    }

    public Guid Id { get; private set; }
    public string Status { get; set; }
    public Duration Duration { get; }
    public IEnumerable<IWorkable> GetTasksForDate(DateTime date)
    {
        throw new NotImplementedException();
    }

    public void ReduceEffortBy(Duration capacity)
    {
        throw new NotImplementedException();
    }
    public TimeSpan Estimate { get; }
}