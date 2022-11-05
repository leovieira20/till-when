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
        await Runner
            .AddSteps(
                _ => GivenAnInexistentQueue())
            .AddAsyncSteps(_ => WhenTheActionIsExecuted())
            .AddSteps(_ => TheDurationShouldBe(Duration.Zero()))
            .RunAsync();
    }
    
    [Scenario]
    public async Task DurationForAnEmptyQueue()
    {
        await Runner
            .AddSteps(
                _ => GivenAQueueWithNoWorkables())
            .AddAsyncSteps(_ => WhenTheActionIsExecuted())
            .AddSteps(_ => TheDurationShouldBe(Duration.Zero()))
            .RunAsync();
    }
    
    [Scenario]
    public async Task CalculateDurationForOneWorkable()
    {
        await Runner
            .AddSteps(
                _ => GivenAQueueWithWorkables(Table.For(
                    (IWorkable)Workable.Create("Workable 01", "1d")
                )))
            .AddAsyncSteps(_ => WhenTheActionIsExecuted())
            .AddSteps(_ => TheDurationShouldBe("1d"))
            .RunAsync();
    }
    
    [Scenario]
    public async Task CalculateDurationForMultipleWorkables()
    {
        await Runner
            .AddSteps(
                _ => GivenAQueueWithWorkables(Table.For(
                    (IWorkable) Workable.Create("Workable 01", "1d"),
                    Workable.Create("Workable 01", "2h"),
                    Workable.Create("Workable 01", "3m")
                )))
            .AddAsyncSteps(_ => WhenTheActionIsExecuted())
            .AddSteps(_ => TheDurationShouldBe("1d 2h 3m"))
            .RunAsync();
    }
}