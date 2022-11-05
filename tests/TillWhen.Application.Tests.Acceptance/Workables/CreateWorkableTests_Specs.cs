using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace TillWhen.Application.Tests.Acceptance.Workables;

[FeatureDescription("As a user I want to be able to create a new workable item")]
public partial class CreateWorkableTests
{
    [Scenario]
    public async Task ValidWorkable()
    {
        await Runner
            .AddSteps(
                _ => GivenValidWorkableSpecs()
            )
            .AddAsyncSteps(
                _ => WhenActionIsExecuted()
            )
            .AddSteps(
                _ => ThenWorkableIsCreated()
            )
            .RunAsync();
    }
}