using System;
using System.Collections.Generic;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.WorkableAggregate;

public class Workable : IWorkable
{
    public static Workable Create(string title, Duration duration) => new(title, duration);
    public static Workable Create(string title, string category, Duration duration) => new(title, category, duration);
    
    private Workable(string title, Duration duration)
        : this(title, string.Empty, duration)
    {
    }

    private Workable(string title, string category, Duration estimation)
    {
        Title = title;
        Category = category;
        Estimation = estimation;
        RemainingEffort = estimation;
        StartingDate = DateTime.UtcNow.Date;
    }

    public IList<IWorkableSplit> GetSplitsFor(Duration capacity)
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
    
    public bool HasRemainingEffort()
    {
        return RemainingEffort > Duration.Zero();
    }

    public Guid Id { get; set; }

    private DateTime StartingDate { get; init; }

    public string Title { get; private set; }

    public string Category { get; private set; }

    
    public Duration Estimation { get; private set; }
    public Duration RemainingEffort { get; set; }
    public Duration ScheduledEffort { get; set; } = Duration.Zero();
}