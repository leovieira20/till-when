using System;
using System.Collections.Generic;
using System.Linq;

namespace TillWhen.Domain.Entities
{
    public class Project
    {
        private TimeSpan _quota;
        private readonly Dictionary<DateTime, List<Task>> _tasksPerDay;

        public static Project Create()
        {
            return new();
        }

        public static Project WithStartingDate(DateTime startingDate)
        {
            return new() { StartingDate = startingDate };
        }

        public static Project WithQuota(TimeSpan quota)
        {
            return new()
            {
                _quota = quota
            };
        }

        private Project()
        {
            _quota = TimeSpan.FromHours(16);
            StartingDate = DateTime.UtcNow.Date;
            _tasksPerDay = new() { { StartingDate, new() } };
            Tasks = new();
        }

        public void AddTask(Task task)
        {
            var lastDayWithEnoughCapacity = _tasksPerDay
                .Keys
                .OrderByDescending(x => x.Date)
                .FirstOrDefault();

            var tasksEstimationForDate = _tasksPerDay[lastDayWithEnoughCapacity]
                .Sum(x => x.GetEstimate());

            var dailyQuotaLeft = _quota.TotalMinutes - tasksEstimationForDate;
            if (task.GetEstimate() <= dailyQuotaLeft)
            {
                _tasksPerDay[lastDayWithEnoughCapacity].Add(task);

                task.SetStartDate(lastDayWithEnoughCapacity);
                Tasks.Add(task);
            }
            else if (dailyQuotaLeft == 0)
            {
                _tasksPerDay.Add(lastDayWithEnoughCapacity.AddDays(1), new());
                AddTask(task);
            }
            else
            {
                var taskWithReducedEstimate = task.ReduceEstimate(dailyQuotaLeft);
                taskWithReducedEstimate.SetStartDate(lastDayWithEnoughCapacity);
                
                _tasksPerDay[lastDayWithEnoughCapacity].Add(taskWithReducedEstimate);
                Tasks.Add(taskWithReducedEstimate);
                
                _tasksPerDay.Add(lastDayWithEnoughCapacity.AddDays(1), new());
                
                AddTask(taskWithReducedEstimate);
            }
        }

        public TimeSpan Estimate
        {
            get
            {
                var totalEstimation = 0.0;
                foreach (var date in _tasksPerDay.Keys)
                {
                    var estimationForDay = _tasksPerDay[date]
                        .Where(x => x.Status != TaskStatus.Completed)
                        .Sum(x => x.GetEstimate());
                    totalEstimation += estimationForDay;
                }

                return TimeSpan.FromMinutes(totalEstimation);
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
            return Tasks
                .SingleOrDefault(x => x.Id == id)?.StartingDate ?? throw new ArgumentException($"Invalid task id {id}");
        }

        public List<Task> GetTasksForDate(DateTime date)
        {
            return _tasksPerDay[date.Date];
        }

        public Guid Id { get; set; }
        public List<Task> PendingTasks => Tasks.Where(x => x.Status != TaskStatus.Completed).ToList();
        private List<Task> Tasks { get; }
        private DateTime StartingDate { get; init; }
    }
}