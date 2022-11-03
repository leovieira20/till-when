using System;
using System.Collections.Generic;

namespace TillWhen.Domain.Common;

public interface IWorkable
{
    double GetEstimate();
    string Status { get; set; }
    Duration Duration { get; }
    IEnumerable<IWorkable> GetTasksForDate(DateTime date);
}