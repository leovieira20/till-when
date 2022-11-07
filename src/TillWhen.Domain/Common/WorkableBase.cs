using System;
using System.Collections.Generic;

namespace TillWhen.Domain.Common;

public abstract class WorkableBase
{
    internal WorkableBase() => Id = Guid.NewGuid();
    protected WorkableBase(Duration estimation) : this() => Estimation = estimation;

    public Guid Id { get; internal set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public Duration Estimation { get; internal set; } = Duration.Zero();
    public Guid WorkableQueueId { get; set; }
    public abstract IList<IWorkableSplit> GetSplitsFor(Duration capacity);
}