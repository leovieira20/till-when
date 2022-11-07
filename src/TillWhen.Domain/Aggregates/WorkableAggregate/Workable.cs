using System.Collections.Generic;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.WorkableAggregate;

public class Workable : WorkableBase
{
    public static Workable Create(string title, Duration duration) => new(title, duration);
    public static Workable Create(string title, string category, Duration duration) => new(title, category, duration);

    private Workable()
    {
    }

    private Workable(string title, Duration duration)
        : this(title, string.Empty, duration)
    {
    }

    private Workable(string title, string category, Duration estimation)
        : base(estimation)
    {
        Title = title;
        Category = category;
    }

    public override IList<IWorkableSplit> GetSplitsFor(Duration capacity)
    {
        var splits = new List<IWorkableSplit>();

        var original = Estimation;
        while (original > Duration.Zero())
        {
            var (adjusted, remaining) = original.Reduce(capacity);
            splits.Add(new WorkableSplit(Title, adjusted));
            original = remaining;
        }

        return splits;
    }
}