using System;
using System.Collections.Generic;
using System.Linq;

namespace TillWhen.Domain.Entities
{
    public class Project
    {
        private TimeSpan _quota;

        public static Project Create()
        {
            return new() { StartingDate = DateTime.UtcNow };
        }

        public static Project WithStartingDate(DateTime startingDate)
        {
            return new() { StartingDate = startingDate };
        }

        private Project()
        {
            _quota = TimeSpan.FromHours(16);
            Tasks = new();
        }

        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        public TimeSpan Estimate
        {
            get
            {
                var totalTaskMinutes = Tasks
                    .Where(x => x.Status != TaskStatus.Completed)
                    .Sum(x => x.Estimate.TotalMinutes);

                var shares = totalTaskMinutes / _quota.TotalMinutes;
                if (shares <= 1)
                {
                    return TimeSpan.FromMinutes(totalTaskMinutes);
                }

                return TimeSpan.FromMinutes(totalTaskMinutes * shares);
            }
        }

        public void SetQuota(TimeSpan quotaDuration)
        {
            _quota = quotaDuration;
        }

        public void CompleteTask(Guid id)
        {
            var task = Tasks.Single(x => x.Id == id);
            task.Complete();
        }

        public DateTime GetStartingDateForTask(Guid id)
        {
            var remainingQuota = _quota;
            foreach (var t in PendingTasks)
            {
                if (t.Id == id)
                {
                    if (remainingQuota >= t.Estimate)
                    {
                        return StartingDate;
                    }

                    return StartingDate.AddDays(1);
                }

                remainingQuota = remainingQuota.Subtract(t.Estimate);
            }

            throw new ArgumentException($"Invalid task id {id}");
        }

        public List<Task> PendingTasks => Tasks.Where(x => x.Status != TaskStatus.Completed).ToList();

        private List<Task> Tasks { get; }

        private DateTime StartingDate { get; set; }
    }
}