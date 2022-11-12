using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TillWhen.Database.SqlServer;
using TillWhen.Database.SqlServer.Repositories;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;
using TillWhen.Domain.Common;

var provider = CreateInfrastructure();
var queueRepository = new EfWorkableQueueRepository(provider.GetRequiredService<TillWhenContext>());
var queueConfigurationRepository = new EfWorkableQueueConfigurationRepository(provider.GetRequiredService<TillWhenContext>());

var workables = new List<WorkableBase>
{
    Workable.Create("Specification Pattern in C#", "DDD", "1h 27m"),
    Workable.Create("Domain-Driven Design in Practice", "DDD", "4h 19m"),
    Workable.Create("Applying Functional Principles in C# 6", "Code Style", "3h 28m"),
    Workable.Create("DDD and EF Core 3: Preserving Encapsulation", "DDD", "3h 39m"),
    Workable.Create("Clean Architecture: Patterns, Practices, and Principles", "Architecture", "2h 20m"),
    Workable.Create("Modern Software Architecture: Domain Models, CQRS, and Event Sourcing", "Architecture", "4h 25m"),
    Workable.Create("CQRS in Practice", "Architecture", "4h 22m"),
    Workable.Create("React 17: Getting Started", "React", "4h 2m"),
    Workable.Create("Designing React 17 Components", "React", "3h 56m"),
    Workable.Create("Managing React State", "React", "5h 6m"),
    Workable.Create("Styling React Components", "React", "1h 8m"),
    Workable.Create("Server Rendering React 16 Components", "React", "1h 31m"),
    Workable.Create("Testing React 16 Components", "React", "2h 23m"),
    Workable.Create("Implementing Forms in React 17", "React", "1h 48m"),
    Workable.Create("Optimize Performance for React", "React", "56m"),
    Workable.Create("Building Applications with React 17 and Redux", "React", "6h 39m"),
    Workable.Create("Using React 17 Hooks", "React", "3h 21m"),
    Workable.Create("Choosing a React Framework", "React", "1h 6m"),
    Workable.Create("Calling APIs with React 17", "React", "2h 14m"),
    Workable.Create("Managing Large Datasets in React 17", "React", "51m"),
    Workable.Create("Building React Apps with TypeScript", "React", "57m"),
    Workable.Create("React 17 Security: Best Practices", "React", "1h 5m"),
    Workable.Create("Kubernetes for Developers: Core Concepts", "Kubernetes", "4h 36m"),
    Workable.Create("Kubernetes for Developers: Deploying Your Code", "Kubernetes", "3h 4m"),
    Workable.Create("Kubernetes for Developers: Integrating Volumes and Using Multi-container Pods", "Kubernetes", "2h 26m"),
    Workable.Create("Kubernetes for Developers: Moving from Docker Compose to Kubernetes", "Kubernetes", "1h 47m"),
    Workable.Create("Kubernetes for Developers: Moving to the Cloud", "Kubernetes", "1h 3m"),
    Workable.Create("Design Patterns Overview", "Design Patterns", "37m"),
    Workable.Create("C# 8 Design Patterns: Strategy", "Design Patterns", "40m"),
    Workable.Create("C# Design Patterns: Singleton", "Design Patterns", "33m"),
    Workable.Create("C# 8 Design Patterns: Command", "Design Patterns", "26m"),
    Workable.Create("C# Design Patterns: Bridge", "Design Patterns", "31m"),
    Workable.Create("C# Design Patterns: Null Object", "Design Patterns", "13m"),
    Workable.Create("C# Design Patterns: State", "Design Patterns", "43m"),
    Workable.Create("C# 8 Design Patterns: Data Access Patterns", "Design Patterns", "1h 24m"),
    Workable.Create("C# 8 Design Patterns: Mediator", "Design Patterns", "37m"),
    Workable.Create("C# 8 Design Patterns: Chain of Responsibility", "Design Patterns", "40m"),
    Workable.Create("C# Design Patterns: Template Method", "Design Patterns", "34m"),
    Workable.Create("C# Design Patterns: Visitor", "Design Patterns", "27m"),
    Workable.Create("C# Design Patterns: Memento", "Design Patterns", "32m"),
    Workable.Create("C# Design Patterns: Builder", "Design Patterns", "28m"),
    Workable.Create("C# Design Patterns: Prototype", "Design Patterns", "23m"),
    Workable.Create("C# 8 Design Patterns: Factory and Abstract Factory", "Design Patterns", "53m"),
    Workable.Create("C# Design Patterns: Facade", "Design Patterns", "13m"),
    Workable.Create("C# Design Patterns: Decorator", "Design Patterns", "32m"),
    Workable.Create("C# 8 Design Patterns: Composite", "Design Patterns", "33m"),
    Workable.Create("C# Design Patterns: Adapter", "Design Patterns", "24m"),
    Workable.Create("C# Design Patterns: Flyweight", "Design Patterns", "35m"),
    Workable.Create("C# Design Patterns: Proxy", "Design Patterns", "35m"),
};

var queue = WorkableQueue.WithWorkables(workables);

queueRepository.Create(queue);
await queueRepository.CommitAsync();

var queueConfiguration = new WorkableQueueConfiguration
{
    Capacity = "8h",
    WorkableQueueId = queue.Id
};

queueConfigurationRepository.Create(queueConfiguration);
await queueConfigurationRepository.SaveAsync();

ServiceProvider CreateInfrastructure()
{
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    var services = new ServiceCollection()
        .AddDbContext<TillWhenContext>(x => { x.UseSqlServer(config.GetConnectionString("TillWhen")); });

    return services.BuildServiceProvider();
}