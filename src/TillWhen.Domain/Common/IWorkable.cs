using System.Collections.Generic;
using TillWhen.Domain.Aggregates.WorkableAggregate;

namespace TillWhen.Domain.Common;

public interface IWorkable
{
    Duration Estimation { get; }
    Duration ScheduledEffort { get; }
    Duration RemainingEffort { get; }
    IWorkable ScheduleEffortBy(Duration scheduledEffort);
    bool HasRemainingEffort();
    IList<IWorkable> GetSplitsFor(Duration capacity);
}