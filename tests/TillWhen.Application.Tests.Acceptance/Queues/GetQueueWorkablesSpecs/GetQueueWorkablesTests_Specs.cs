using LightBDD.Framework;
using LightBDD.Framework.Parameters;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Queues.GetQueueWorkablesSpecs;

[FeatureDescription("")]
public partial class GetQueueWorkablesTests
{
    [Scenario]
    public Task ListQueueWorkables()
    {
        IWorkable wOne = Workable.Create("Workable 1", "1h");
        IWorkable wTwo = Workable.Create("Workable 2", "1h");

        return Runner.RunScenarioAsync(
            _ => GivenAQueueWithWorkables(Table.For(wOne, wTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfTasksForTodayIsReturned(Table.For(wOne, wTwo))
        );
    }

    [Scenario]
    public Task BreakQueueWorkablesPerDayOf16H()
    {
        IWorkable wOne = Workable.Create("Workable 1", "16h");
        IWorkable wTwo = Workable.Create("Workable 2", "1h");

        return Runner.RunScenarioAsync(
            _ => GivenAQueueWithWorkables(Table.For(wOne, wTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfQueueDaysIsReturned(Table.For(
                QueueDay.WithWorkables(DateTime.UtcNow, new() { wOne }),
                QueueDay.WithWorkables(DateTime.UtcNow.AddDays(1), new() { wTwo })
            ))
        );
    }
    
    [Scenario]
    public Task BreakQueueTasksPerDayOf8H()
    {
        IWorkable taskOne = Workable.Create("Task 1", "8h");
        IWorkable taskTwo = Workable.Create("Task 2", "8h");

        return Runner.RunScenarioAsync(
            _ => GivenAQueueSetUpFor8HWithWorkables(Table.For(taskOne, taskTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfQueueDaysIsReturned(Table.For(
                QueueDay.WithWorkables(DateTime.UtcNow, new() { taskOne }),
                QueueDay.WithWorkables(DateTime.UtcNow.AddDays(1), new() { taskTwo })
            ))
        );
    }
}