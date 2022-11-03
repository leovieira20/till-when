using System;
using System.Collections.Generic;
using System.Linq;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.ProjectAggregate;

public class Project : IWorkable
{
    private TimeSpan _quota;
    private readonly Dictionary<DateTime, List<IWorkable>> _tasksPerDay;

    public static Project Create() => new();
    public static Project Create(string title, Duration duration) => new(title, duration);
    public static Project Create(string title, string category, Duration duration) => new(title, category, duration);
    public static Project WithStartingDate(DateTime startingDate) => new() { StartingDate = startingDate };
    public static Project WithQuota(TimeSpan quota) => new() { _quota = quota };

    private Project()
        : this(string.Empty, string.Empty, Duration.Empty())
    {
    }

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
        _quota = TimeSpan.FromHours(16);
        StartingDate = DateTime.UtcNow.Date;
        _tasksPerDay = new() { { StartingDate, new() } };
        Tasks = new();
    }

    public void AddTask(ProjectTask task)
    {
        var lastDayWithEnoughCapacity = _tasksPerDay
            .Keys
            .OrderByDescending(x => x.Date)
            .FirstOrDefault();

        var tasksEstimationForDate = _tasksPerDay[lastDayWithEnoughCapacity]
            .Sum(x => x.GetEstimate());

        var dailyQuotaLeft = _quota.TotalMinutes - tasksEstimationForDate;
        if (task.GetEstimate() <= dailyQuotaLeft)
        {
            _tasksPerDay[lastDayWithEnoughCapacity].Add(task);

            task.SetStartDate(lastDayWithEnoughCapacity);
            Tasks.Add(task);
        }
        else if (dailyQuotaLeft == 0)
        {
            _tasksPerDay.Add(lastDayWithEnoughCapacity.AddDays(1), new());
            AddTask(task);
        }
        else
        {
            var taskWithReducedEstimate = task.ReduceEstimate(dailyQuotaLeft);
            taskWithReducedEstimate.SetStartDate(lastDayWithEnoughCapacity);

            _tasksPerDay[lastDayWithEnoughCapacity].Add(taskWithReducedEstimate);
            Tasks.Add(taskWithReducedEstimate);

            _tasksPerDay.Add(lastDayWithEnoughCapacity.AddDays(1), new());

            AddTask(taskWithReducedEstimate);
        }
    }

    public TimeSpan Estimate
    {
        get
        {
            var totalEstimation = 0.0;
            foreach (var date in _tasksPerDay.Keys)
            {
                var estimationForDay = _tasksPerDay[date]
                    .Where(x => x.Status != TaskStatus.Completed)
                    .Sum(x => x.GetEstimate());
                totalEstimation += estimationForDay;
            }

            return TimeSpan.FromMinutes(totalEstimation);
        }
    }

    public void SetQuota(TimeSpan quotaDuration)
    {
        _quota = quotaDuration;
    }

    public void CompleteTask(Guid id)
    {
        var task = Tasks.Single(x => x.Id == id);
        task.Complete();
    }

    public DateTime GetStartingDateForTask(Guid id)
    {
        return Tasks
            .SingleOrDefault(x => x.Id == id)?.StartingDate ?? throw new ArgumentException($"Invalid task id {id}");
    }

    public List<IWorkable> GetTasksForDate(DateTime date)
    {
        return _tasksPerDay[date.Date];
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

    IEnumerable<IWorkable> IWorkable.GetTasksForDate(DateTime date)
    {
        return GetTasksForDate(date);
    }

    public Guid TaskQueueId { get; set; }

    public Duration RemainingEffort { get; set; }
    public List<ProjectTask> PendingTasks => Tasks.Where(x => x.Status != TaskStatus.Completed).ToList();
    private List<ProjectTask> Tasks { get; }
}