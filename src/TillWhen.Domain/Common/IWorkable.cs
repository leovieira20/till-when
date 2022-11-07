using System;
using System.Collections.Generic;

namespace TillWhen.Domain.Common;

public interface IWorkable
{
    public Guid Id { get; set; }
    Duration Estimation { get; }
    IList<IWorkableSplit> GetSplitsFor(Duration capacity);
}