using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace TillWhen.Application.Acceptance.Projects;

[FeatureDescription("As a user I want to be able to create a new project")]
public partial class CreateProjectTests
{
    [Scenario]
    public void ValidProject()
    {
        Runner.RunScenario(
            _ => GivenValidProjectSpecs(),
            _ => WhenActionIsExecuted(),
            _ => ThenProjectIsCreated()
        );
    }
}