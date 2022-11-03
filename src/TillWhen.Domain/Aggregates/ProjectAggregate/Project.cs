using System;
using System.Collections.Generic;
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
    public void ReduceEffortBy(Duration capacity)
    {
        if (capacity > RemainingEffort)
        {
            RemainingEffort = Duration.Empty();
        }
        else
        {
            RemainingEffort -= capacity;    
        }
    }

    public Guid Id { get; set; }
    private DateTime StartingDate { get; init; }
    public string Title { get; private set; }
    public string Category { get; private set; }

    public double GetEstimate()
    {
        throw new NotImplementedException();
    }

    public string Status { get; set; }
    public Duration Duration { get; private set; }
    public Duration RemainingEffort { get; set; }
    private List<ProjectTask> Tasks { get; } = new();
}