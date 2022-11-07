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
        WorkableBase wOne = Workable.Create("Workable 1", "1h");
        WorkableBase wTwo = Workable.Create("Workable 2", "1h");

        return Runner.RunScenarioAsync(
            _ => GivenAQueueWithWorkables(Table.For(wOne, wTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfTasksForTodayIsReturned(Table.For(
                new WorkableSplit("Workable 1", "1h"), 
                new WorkableSplit("Workable 2", "1h")))
        );
    }

    [Scenario]
    public Task BreakQueueWorkablesPerDayOf16H()
    {
        WorkableBase wOne = Workable.Create("Workable 1", "16h");
        WorkableBase wTwo = Workable.Create("Workable 2", "1h");

        var sOne = new WorkableSplit("Workable 1", "16h");
        var sTwo = new WorkableSplit("Workable 2", "1h");

        return Runner.RunScenarioAsync(
            _ => GivenAQueueWithWorkables(Table.For(wOne, wTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfQueueDaysIsReturned(Table.For(
                QueueDay.WithWorkables(DateTime.UtcNow, new() { sOne }),
                QueueDay.WithWorkables(DateTime.UtcNow.AddDays(1), new() { sTwo })
            ))
        );
    }
    
    [Scenario]
    public Task BreakQueueTasksPerDayOf8H()
    {
        WorkableBase taskOne = Workable.Create("Workable 1", "8h");
        WorkableBase taskTwo = Workable.Create("Workable 2", "8h");
        
        var sOne = new WorkableSplit("Workable 1", "8h");
        var sTwo = new WorkableSplit("Workable 2", "8h");

        return Runner.RunScenarioAsync(
            _ => GivenAQueueSetUpFor8HWithWorkables(Table.For(taskOne, taskTwo)),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfQueueDaysIsReturned(Table.For(
                QueueDay.WithWorkables(DateTime.UtcNow, new() { sOne }),
                QueueDay.WithWorkables(DateTime.UtcNow.AddDays(1), new() { sTwo })
            ))
        );
    }
}