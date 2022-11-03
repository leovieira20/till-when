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
        IWorkable projectOne = Project.Create("Task 1", Duration.Create("1h"));
        IWorkable projectTwo = Project.Create("Task 2", Duration.Create("1h"));

        return Runner.RunScenarioAsync(
            _ => GivenAQueueWithTasks(Table.For(projectOne, projectTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfTasksForTodayIsReturned()
        );
    }

    [Scenario]
    public Task BreakQueueTasksPerDay()
    {
        IWorkable projectOne = Project.Create("Task 1", Duration.Create("24h"));
        IWorkable projectTwo = Project.Create("Task 2", Duration.Create("24h"));

        return Runner.RunScenarioAsync(
            _ => GivenAQueueWithTasks(Table.For(projectOne, projectTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfQueueDaysIsReturned(Table.For(
                QueueDay.WithTasks(DateTime.UtcNow, new() { projectOne }),
                QueueDay.WithTasks(DateTime.UtcNow.AddDays(1), new() { projectTwo })
            ))
        );
    }
}