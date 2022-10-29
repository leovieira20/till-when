using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.QueueAggregate;

public class TaskQueue
{
    private TaskQueue() { Projects = new(); }
    
    public static TaskQueue Empty()
    {
        return new();
    }

    public static TaskQueue WithProjects(List<Project> projects)
    {
        return new()
        {
            Projects = projects
        };
    }
    
    public Duration CalculateDuration()
    {
        if (!Projects.Any())
        {
            return Duration.Empty();
        }
        
        return Projects
            .Aggregate(Duration.Empty(), (current, p) => current + p.Duration);
    }

    public QueueDay GetTasksForDate(DateTime date)
    {
        if (!Projects.Any())
        {
            return QueueDay.Default();
        }

        var tasks = Projects.SelectMany(x => x.GetTasksForDate(date));

        return QueueDay.WithTasks(tasks.ToList());
    }

    public Guid Id { get; set; }
    public List<Project> Projects { get; private set; }
}