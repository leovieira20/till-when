using LightBDD.Framework;
using LightBDD.Framework.Parameters;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Queues.GetQueueTasksSpecs;

[FeatureDescription("")]
public partial class GetQueueProjectsTests
{
    [Scenario]
    public Task ListQueueTasks()
    {
        IWorkable taskOne = Project.Create("Task 1", Duration.Create("1h"));
        IWorkable taskTwo = Project.Create("Task 2", Duration.Create("1h"));

        return Runner.RunScenarioAsync(
            _ => GivenAQueueWithTasks(Table.For(taskOne, taskTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfTasksForTodayIsReturned(Table.For(taskOne, taskTwo))
        );
    }

    [Scenario]
    public Task BreakQueueTasksPerDayOf16H()
    {
        IWorkable taskOne = Project.Create("Task 1", Duration.Create("16h"));
        IWorkable taskTwo = Project.Create("Task 2", Duration.Create("1h"));

        return Runner.RunScenarioAsync(
            _ => GivenAQueueWithTasks(Table.For(taskOne, taskTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfQueueDaysIsReturned(Table.For(
                QueueDay.WithTasks(DateTime.UtcNow, new() { taskOne }),
                QueueDay.WithTasks(DateTime.UtcNow.AddDays(1), new() { taskTwo })
            ))
        );
    }
    
    [Scenario]
    public Task BreakQueueTasksPerDayOf8H()
    {
        IWorkable taskOne = Project.Create("Task 1", Duration.Create("8h"));
        IWorkable taskTwo = Project.Create("Task 2", Duration.Create("8h"));

        return Runner.RunScenarioAsync(
            _ => GivenAQueueSetUpFor8HWithTasks(Table.For(taskOne, taskTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfQueueDaysIsReturned(Table.For(
                QueueDay.WithTasks(DateTime.UtcNow, new() { taskOne }),
                QueueDay.WithTasks(DateTime.UtcNow.AddDays(1), new() { taskTwo })
            ))
        );
    }
}