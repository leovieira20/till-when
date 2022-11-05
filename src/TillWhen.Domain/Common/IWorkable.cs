using TillWhen.Domain.Aggregates.ProjectAggregate;

namespace TillWhen.Domain.Common;

public interface IWorkable
{
    Duration Duration { get; }
    Duration ScheduledDuration { get; }
    Project ScheduleEffortBy(Duration scheduledEffort);
    bool HasRemainingEffort();
}