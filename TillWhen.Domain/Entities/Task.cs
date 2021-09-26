using System;

namespace TillWhen.Domain.Entities
{
    public record Task
    {
        public Task(Guid id, TimeSpan estimate)
        {
            Id = id;
            Status = TaskStatus.Pending;
            Estimate = estimate;
        }

        public double GetEstimate()
        {
            return Estimate.TotalMinutes;
        }

        public Task ReduceEstimate(double dailyQuotaLeft)
        {
            return new(Id, Estimate.Subtract(TimeSpan.FromMinutes(dailyQuotaLeft)))
            {
                Status = Status
            };
        }

        public void Complete()
        {
            Status = TaskStatus.Completed;
        }

        public void SetStartDate(DateTime startingDate)
        {
            StartingDate = startingDate;
        }

        public Guid Id { get; }
        public string Status { get; private set; }
        public DateTime StartingDate { get; private set; }
        public TimeSpan Estimate { get; }
    }
}