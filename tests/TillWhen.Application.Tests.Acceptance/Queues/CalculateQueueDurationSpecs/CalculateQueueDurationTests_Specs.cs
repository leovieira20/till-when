using LightBDD.Framework;
using LightBDD.Framework.Parameters;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Queues.CalculateQueueDurationSpecs;

[FeatureDescription("As a user I want to be able to calculate the duration of a queue")]
public partial class CalculateQueueDurationTests
{
    [Scenario]
    public async Task DurationForAnInexistentQueue()
    {
        await Runner.RunScenarioAsync(
            _ => GivenAnInexistentQueue(),
            _ => WhenTheActionIsExecuted(),
            _ => TheDurationShouldBe(Duration.Zero())
        );
    }

    [Scenario]
    public async Task DurationForAnEmptyQueue()
    {
        await Runner.RunScenarioAsync(
            _ => GivenAQueueWithNoWorkables(),
            _ => WhenTheActionIsExecuted(),
            _ => TheDurationShouldBe(Duration.Zero())
        );
    }

    [Scenario]
    public async Task CalculateDurationForOneWorkable()
    {
        await Runner.RunScenarioAsync(
            _ => GivenAQueueWithWorkables(Table.For(
                (WorkableBase)Workable.Create("Workable 01", "1d")
            )),
            _ => WhenTheActionIsExecuted(),
            _ => TheDurationShouldBe("1d"));
    }

    [Scenario]
    public async Task CalculateDurationForMultipleWorkables()
    {
        await Runner.RunScenarioAsync(
            _ => GivenAQueueWithWorkables(Table.For(
                (WorkableBase)Workable.Create("Workable 01", "1d"),
                Workable.Create("Workable 01", "2h"),
                Workable.Create("Workable 01", "3m")
            )),
            _ => WhenTheActionIsExecuted(),
            _ => TheDurationShouldBe("1d 2h 3m"));
    }
}