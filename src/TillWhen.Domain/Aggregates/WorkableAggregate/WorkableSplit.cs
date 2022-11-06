using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.WorkableAggregate;

internal record WorkableSplit(string Title, Duration Estimation) : IWorkableSplit
{
    public bool HasRemainingEffort()
    {
        return Estimation > Duration.Zero();
    }

    public WorkableSplit ReduceEstimationBy(Duration availableEffort)
    {
        if (availableEffort >= Estimation)
        {
            return this with { Estimation = availableEffort };
        }

        return this with { Estimation = Estimation - availableEffort };
    }
    
    public Duration Estimation { get; private init; } = Estimation;
}