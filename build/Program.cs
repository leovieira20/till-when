using System.Threading.Tasks;
using Cake.Common.Tools.DotNet;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public BuildContext(ICakeContext context)
        : base(context)
    {
    }
}

[TaskName("Restore")]
public sealed class RestoreTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetRestore("../TillWhen.sln");
    }
}

[TaskName("Build")]
[IsDependentOn(typeof(RestoreTask))]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetBuild("../src/TillWhen.Api/TillWhen.Api.csproj");
    }
}

[TaskName("Domain-Test")]
[IsDependentOn(typeof(BuildTask))]
public sealed class DomainTestsTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetTest("../tests/TillWhen.Domain.Tests.Unit/TillWhen.Domain.Tests.Unit.csproj");
    }
}

[TaskName("Application-Test")]
[IsDependentOn(typeof(BuildTask))]
public sealed class ApplicationTestsTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetTest("../tests/TillWhen.Application.Tests.Acceptance/TillWhen.Application.Tests.Acceptance.csproj");
    }
}

[TaskName("Api-Test")]
[IsDependentOn(typeof(BuildTask))]
public sealed class ApiTestsTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetTest("../tests/TillWhen.Api.Tests.Integration/TillWhen.Api.Tests.Integration.csproj");
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(DomainTestsTask))]
[IsDependentOn(typeof(ApplicationTestsTask))]
[IsDependentOn(typeof(ApiTestsTask))]
public class DefaultTask : FrostingTask
{
}