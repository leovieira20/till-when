using System;
using System.Collections.Generic;
using FluentAssertions;
using TillWhen.Domain.Entities;
using ProjectEntity = TillWhen.Domain.Entities.Project;

namespace TillWhen.Domain.Tests.Entities.Project
{
    public partial class ProjectTestsSteps
    {
        private ProjectEntity _project;
        private TimeSpan _projectEstimate;
        private readonly Dictionary<Guid, DateTime> _startingDates;
        private Exception _exception;
        private Guid _taskId;

        public ProjectTestsSteps()
        {
            _startingDates = new();
        }
        
        private void GivenProjectWithStartingDate(DateTime startingDate)
        {
            _project = ProjectEntity.WithStartingDate(startingDate);
        }

        private void GivenProject()
        {
            _project = ProjectEntity.Create();
        }

        private void AndProjectHasPendingTaskOfDuration(TimeSpan duration)
        {
            var taskId = Guid.NewGuid();
            
            Task task = new(taskId, duration);
            
            _taskId = taskId;
            
            _project.AddTask(task);
        }

        private void AndProjectHasPendingTaskWithId(Guid id)
        {
            Task task = new(id, TimeSpan.FromHours(1));
            _project.AddTask(task);
        }

        private void AndProjectHasQuotaOf(TimeSpan quotaDuration)
        {
            _project.SetQuota(quotaDuration);
        }

        private void AndProjectHasCompletedTaskWithDurationOf(TimeSpan duration)
        {
            Task task = new(new(), duration);
            task.Complete();

            _project.AddTask(task);
        }

        private void AndProjectHasPendingTaskWithIdAndEstimation(Guid id, TimeSpan duration)
        {
            Task task = new(id, duration);
            _project.AddTask(task);
        }

        private void WhenICompleteTaskWithId(Guid id)
        {
            _project.CompleteTask(id);
        }

        private void WhenIAskForEstimatedStartingDateForTask(Guid id)
        {
            try
            {
                _startingDates[id] = _project.GetStartingDateForTask(id);
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        private void WhenCalculatingTheEstimationOfTheProject()
        {
            _projectEstimate = _project.Estimate;
        }

        private void ThenEstimateShouldBe(TimeSpan expectedDuration)
        {
            _projectEstimate.Should().Be(expectedDuration);
        }

        private void ThenProjectHasOnePendingTaskLeft()
        {
            _project.PendingTasks.Count.Should().Be(1);
        }

        private void ThenEstimatedDateTimeShouldBe(Guid taskId, DateTime estimatedDate)
        {
            _startingDates[taskId].Date.Should().Be(estimatedDate.Date);
        }

        private void ExceptionIsThrownWithMessage(string message)
        {
            _exception.Message.Should().Be(message);
        }
    }
}