using System;
using TillWhen.Domain.Common;

namespace TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;

public class WorkableQueueConfiguration
{
    public Guid Id { get; set; }
    public Duration Capacity { get; set; }

    public Guid WorkableQueueId { get; set; }
}