using TillWhen.Domain.Aggregates.WorkableAggregate;

namespace TillWhen.Domain.Common;

public interface IWorkable
{
    Duration Duration { get; }
    Duration ScheduledDuration { get; }
    Workable ScheduleEffortBy(Duration scheduledEffort);
    bool HasRemainingEffort();
}