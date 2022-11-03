
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TillWhen.Database.SqlServer;
using TillWhen.Database.SqlServer.Repositories;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Common;

var projects = new List<IWorkable>
{
    Project.Create("Specification Pattern in C#", "DDD", "1h 27m"),
    Project.Create("Domain-Driven Design in Practice", "DDD", "4h 19m"),
    Project.Create("Applying Functional Principles in C# 6", "Code Style", "3h 28m"),
    Project.Create("Refactoring from Anemic Domain Model Towards a Rich One", "Code Style", "3h 36m"),
    Project.Create("DDD and EF Core 3: Preserving Encapsulation", "DDD", "3h 39m"),
    Project.Create("Clean Architecture: Patterns, Practices, and Principles", "Architecture", "2h 20m"),
    Project.Create("Modern Software Architecture: Domain Models, CQRS, and Event Sourcing", "Architecture", "4h 25m"),
    Project.Create("CQRS in Practice", "Architecture", "4h 22m"),
    Project.Create("React 17: Getting Started", "React", "4h 2m"),
    Project.Create("Designing React 17 Components", "React", "3h 56m"),
    Project.Create("Managing React State", "React", "5h 6m"),
    Project.Create("Styling React Components", "React", "1h 8m"),
    Project.Create("Server Rendering React 16 Components", "React", "1h 31m"),
    Project.Create("Testing React 16 Components", "React", "2h 23m"),
    Project.Create("Implementing Forms in React 17", "React", "1h 48m"),
    Project.Create("Optimize Performance for React", "React", "56m"),
    Project.Create("Building Applications with React 17 and Redux", "React", "6h 39m"),
    Project.Create("Using React 17 Hooks", "React", "3h 21m"),
    Project.Create("Choosing a React Framework", "React", "1h 6m"),
    Project.Create("Calling APIs with React 17", "React", "2h 14m"),
    Project.Create("Managing Large Datasets in React 17", "React", "51m"),
    Project.Create("Building React Apps with TypeScript", "React", "57m"),
    Project.Create("React 17 Security: Best Practices", "React", "1h 5m"),
    Project.Create("Kubernetes for Developers: Core Concepts", "Kubernetes", "4h 36m"),
    Project.Create("Kubernetes for Developers: Deploying Your Code", "Kubernetes", "3h 4m"),
    Project.Create("Kubernetes for Developers: Integrating Volumes and Using Multi-container Pods", "Kubernetes", "2h 26m"),
    Project.Create("Kubernetes for Developers: Moving from Docker Compose to Kubernetes", "Kubernetes", "1h 47m"),
    Project.Create("Kubernetes for Developers: Moving to the Cloud", "Kubernetes", "1h 3m"),
    Project.Create("Design Patterns Overview", "Design Patterns", "37m"),
    Project.Create("C# 8 Design Patterns: Strategy", "Design Patterns", "40m"),
    Project.Create("C# Design Patterns: Singleton", "Design Patterns", "33m"),
    Project.Create("C# 8 Design Patterns: Command", "Design Patterns", "26m"),
    Project.Create("C# Design Patterns: Bridge", "Design Patterns", "31m"),
    Project.Create("C# Design Patterns: Null Object", "Design Patterns", "13m"),
    Project.Create("C# Design Patterns: State", "Design Patterns", "43m"),
    Project.Create("C# 8 Design Patterns: Data Access Patterns", "Design Patterns", "1h 24m"),
    Project.Create("C# 8 Design Patterns: Mediator", "Design Patterns", "37m"),
    Project.Create("C# 8 Design Patterns: Chain of Responsibility", "Design Patterns", "40m"),
    Project.Create("C# Design Patterns: Template Method", "Design Patterns", "34m"),
    Project.Create("C# Design Patterns: Visitor", "Design Patterns", "27m"),
    Project.Create("C# Design Patterns: Memento", "Design Patterns", "32m"),
    Project.Create("C# Design Patterns: Builder", "Design Patterns", "28m"),
    Project.Create("C# Design Patterns: Prototype", "Design Patterns", "23m"),
    Project.Create("C# 8 Design Patterns: Factory and Abstract Factory", "Design Patterns", "53m"),
    Project.Create("C# Design Patterns: Facade", "Design Patterns", "13m"),
    Project.Create("C# Design Patterns: Decorator", "Design Patterns", "32m"),
    Project.Create("C# 8 Design Patterns: Composite", "Design Patterns", "33m"),
    Project.Create("C# Design Patterns: Adapter", "Design Patterns", "24m"),
    Project.Create("C# Design Patterns: Flyweight", "Design Patterns", "35m"),
    Project.Create("C# Design Patterns: Proxy", "Design Patterns", "35m"),
};

var queue = TaskQueue.WithTasks(projects);

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection()
    .AddDbContext<TillWhenContext>(x =>
    {
        x.UseSqlServer(config.GetConnectionString("TillWhen"));
    });

var provider = services.BuildServiceProvider();

var queueRepository = new EfTaskQueueRepository(provider.GetRequiredService<TillWhenContext>());

queueRepository.Create(queue);
await queueRepository.CommitAsync();