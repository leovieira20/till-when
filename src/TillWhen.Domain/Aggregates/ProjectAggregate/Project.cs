using System;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.ProjectAggregate;

public class Project : IWorkable
{
    public static Project Create(string title, Duration duration) => new(title, duration);
    public static Project Create(string title, string category, Duration duration) => new(title, category, duration);
    
    private Project(string title, Duration duration)
        : this(title, string.Empty, duration)
    {
    }

    private Project(string title, string category, Duration duration)
    {
        Title = title;
        Category = category;
        Duration = duration;
        RemainingEffort = duration;
        StartingDate = DateTime.UtcNow.Date;
    }

    public bool HasRemainingEffort()
    {
        return RemainingEffort > Duration.Zero();
    }

    public Duration ScheduledDuration => ScheduledEffort;

    public Project ScheduleEffortBy(Duration scheduledEffort)
    {
        if (scheduledEffort > RemainingEffort)
        {
            ScheduledEffort = RemainingEffort;
            RemainingEffort = Duration.Zero();
        }
        else
        {
            ScheduledEffort = scheduledEffort;
            RemainingEffort -= scheduledEffort;
        }

        return Create(Title, Category, RemainingEffort);
    }
    
    public Guid Id { get; set; }

    private DateTime StartingDate { get; init; }

    public string Title { get; private set; }

    public string Category { get; private set; }

    
    public Duration Duration { get; private set; }
    public Duration RemainingEffort { get; set; }
    public Duration ScheduledEffort { get; set; } = Duration.Zero();
}