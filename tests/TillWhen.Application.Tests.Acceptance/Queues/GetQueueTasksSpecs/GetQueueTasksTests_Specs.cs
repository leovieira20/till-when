using LightBDD.Framework;
using LightBDD.Framework.Parameters;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Queues.GetQueueTasksSpecs;

[FeatureDescription("")]
public partial class GetQueueProjectsTests
{
    [Scenario]
    public Task ListQueueTasks()
    {
        return Runner.RunScenarioAsync(
            _ => GivenAQueueWithTasks(
                Table.For(
                    (IWorkable)Project.Create("Project 1", Duration.Create("1h")),
                    Project.Create("Project 2", Duration.Create("1h"))
                )),
            _ => WhenHandlerIsExecuted(),
            _ => ThenAListOfTasksForTodayIsReturned()
        );
    }
}