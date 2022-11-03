using System;
using System.Collections.Generic;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.ProjectAggregate;

public record ProjectTask : IWorkable
{
    private ProjectTask() { }
    public ProjectTask(Guid id, TimeSpan estimate)
    {
        Id = id;
        Status = TaskStatus.Pending;
        Estimate = estimate;
    }

    public double GetEstimate()
    {
        return Estimate.TotalMinutes;
    }

    public ProjectTask ReduceEstimate(double dailyQuotaLeft)
    {
        return new(Id, Estimate.Subtract(TimeSpan.FromMinutes(dailyQuotaLeft)))
        {
            Status = Status
        };
    }

    public void Complete()
    {
        Status = TaskStatus.Completed;
    }

    public void SetStartDate(DateTime startingDate)
    {
        StartingDate = startingDate;
    }

    public Guid Id { get; private set; }
    public string Status { get; set; }
    public Duration Duration { get; }
    public IEnumerable<IWorkable> GetTasksForDate(DateTime date)
    {
        throw new NotImplementedException();
    }

    string IWorkable.Status
    {
        get => this.Status;
        set => this.Status = value;
    }

    public DateTime StartingDate { get; private set; }
    public TimeSpan Estimate { get; }
}