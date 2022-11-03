using LightBDD.Framework;
using LightBDD.Framework.Parameters;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using TillWhen.Domain.Aggregates.ProjectAggregate;
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
            .AddSteps(_ => TheDurationShouldBe(Duration.Empty()))
            .RunAsync();
    }
    
    [Scenario]
    public async Task DurationForAnEmptyQueue()
    {
        await Runner
            .AddSteps(
                _ => GivenAQueueWithNoProjects())
            .AddAsyncSteps(_ => WhenTheActionIsExecuted())
            .AddSteps(_ => TheDurationShouldBe(Duration.Empty()))
            .RunAsync();
    }
    
    [Scenario]
    public async Task CalculateDurationForOneProject()
    {
        await Runner
            .AddSteps(
                _ => GivenAQueueWithProjects(Table.For(
                    (IWorkable)Project.Create("Project 01", Duration.Create("1d"))
                )))
            .AddAsyncSteps(_ => WhenTheActionIsExecuted())
            .AddSteps(_ => TheDurationShouldBe(Duration.Create("1d")))
            .RunAsync();
    }
    
    [Scenario]
    public async Task CalculateDurationForMultipleProjects()
    {
        await Runner
            .AddSteps(
                _ => GivenAQueueWithProjects(Table.For(
                    (IWorkable) Project.Create("Project 01", Duration.Create("1d")),
                    Project.Create("Project 01", Duration.Create("2h")),
                    Project.Create("Project 01", Duration.Create("3m"))
                )))
            .AddAsyncSteps(_ => WhenTheActionIsExecuted())
            .AddSteps(_ => TheDurationShouldBe(Duration.Create("1d 2h 3m")))
            .RunAsync();
    }
}