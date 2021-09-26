using System;

namespace TillWhen.Domain.Entities
{
    public class Task
    {
        public Task()
        {
            Id = Guid.NewGuid();
            Status = TaskStatus.Pending;
        }

        public Task(Guid id)
        {
            Id = id;
            Status = TaskStatus.Pending;
        }

        public void Complete()
        {
            Status = TaskStatus.Completed;
        }

        public Guid Id { get; init; }
        public TimeSpan Estimate { get; init; }
        public string Status { get; private set; }
    }
}