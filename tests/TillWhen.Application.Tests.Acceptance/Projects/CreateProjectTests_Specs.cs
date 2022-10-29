using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace TillWhen.Application.Tests.Acceptance.Projects;

[FeatureDescription("As a user I want to be able to create a new project")]
public partial class CreateProjectTests
{
    [Scenario]
    public async Task ValidProject()
    {
        await Runner
            .AddSteps(
                _ => GivenValidProjectSpecs()
            )
            .AddAsyncSteps(
                _ => WhenActionIsExecuted()
            )
            .AddSteps(
                _ => ThenProjectIsCreated()
            )
            .RunAsync();
    }
}