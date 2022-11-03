using System;
using System.Collections.Generic;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.SimpleTaskAggregate;

public class SimpleTask : IWorkable
{
    public double GetEstimate()
    {
        throw new System.NotImplementedException();
    }

    public string Status { get; set; }
    public Duration Duration { get; }
    public IEnumerable<IWorkable> GetTasksForDate(DateTime date)
    {
        throw new NotImplementedException();
    }
}