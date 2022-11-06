using System.Collections.Generic;
using TillWhen.Domain.Aggregates.WorkableAggregate;

namespace TillWhen.Domain.Common;

public interface IWorkable
{
    Duration Estimation { get; }
    IList<IWorkableSplit> GetSplitsFor(Duration capacity);
}