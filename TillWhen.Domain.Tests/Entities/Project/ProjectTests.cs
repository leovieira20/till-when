using System;
using TestStack.BDDfy;
using Xunit;

namespace TillWhen.Domain.Tests
{
    public partial class ProjectTestsSteps
    {
        [Fact]
        public void ProjectWithOneTask()
        {
            this.Given(x => x.GivenProject())
                .And(x => x.AndProjectHasPendingTaskOfDuration(TimeSpan.FromHours(8)))
                .When(x => x.WhenCalculatingTheEstimationOfTheProject())
                .Then(x => x.ThenEstimateShouldBe(TimeSpan.FromHours(8)))
                .BDDfy();
        }

        [Fact]
        public void ProjectWithTwoTasksOfDifferentEstimations()
        {
            this.Given(x => x.GivenProject())
                .And(x => x.AndProjectHasPendingTaskOfDuration(TimeSpan.FromHours(8)))
                .And(x => x.AndProjectHasPendingTaskOfDuration(TimeSpan.FromHours(4)))
                .When(x => x.WhenCalculatingTheEstimationOfTheProject())
                .Then(x => x.ThenEstimateShouldBe(TimeSpan.FromHours(12)))
                .BDDfy();
        }

        [Fact]
        public void ProjectWithQuotaOf4HoursAndOneTaskOfEightHours()
        {
            this.Given(x => x.GivenProject())
                .And(x => x.AndProjectHasQuotaOf(TimeSpan.FromHours(4)))
                .And(x => x.AndProjectHasPendingTaskOfDuration(TimeSpan.FromHours(8)))
                .When(x => x.WhenCalculatingTheEstimationOfTheProject())
                .Then(x => x.ThenEstimateShouldBe(TimeSpan.FromHours(16)))
                .BDDfy();
        }

        [Fact]
        public void ProjectDoesNotConsiderCompletedTasksWhenEstimating()
        {
            this.Given(x => x.GivenProject())
                .And(x => x.AndProjectHasPendingTaskOfDuration(TimeSpan.FromHours(8)))
                .And(x => x.AndProjectHasCompletedTaskWithDurationOf(TimeSpan.FromHours(8)))
                .When(x => x.WhenCalculatingTheEstimationOfTheProject())
                .Then(x => x.ThenEstimateShouldBe(TimeSpan.FromHours(8)))
                .BDDfy();
        }

        [Fact]
        public void ProjectCompletesTask()
        {
            var firstTaskId = Guid.NewGuid();
            var secondTaskId = Guid.NewGuid();

            this.Given(x => x.GivenProject())
                .And(x => x.AndProjectHasPendingTaskWithId(firstTaskId))
                .And(x => x.AndProjectHasPendingTaskWithId(secondTaskId))
                .When(x => x.WhenICompleteTaskWithId(firstTaskId))
                .Then(x => x.ThenProjectHasOnePendingTaskLeft())
                .BDDfy();
        }

        [Fact]
        public void ProjectDeterminesStartDateOfTask()
        {
            var firstTaskId = Guid.NewGuid();
            var secondTaskId = Guid.NewGuid();

            this.Given(x => x.GivenProjectWithStartingDate(DateTime.UtcNow))
                .And(x => x.AndProjectHasPendingTaskWithIdAndEstimation(firstTaskId, TimeSpan.FromHours(16)))
                .And(x => x.AndProjectHasPendingTaskWithIdAndEstimation(secondTaskId, TimeSpan.FromHours(8)))
                .When(x => x.WhenIAskForEstimatedStartingDateForTask(firstTaskId))
                .Then(x => x.ThenEstimatedDateTimeShouldBe(firstTaskId, DateTime.UtcNow))
                .When(x => x.WhenIAskForEstimatedStartingDateForTask(secondTaskId))
                .Then(x => x.ThenEstimatedDateTimeShouldBe(secondTaskId, DateTime.UtcNow.AddDays(1)))
                .BDDfy();
        }

        [Fact]
        public void ProjectThrowsExceptionWhenTryingToGetAStartingDateFromInvalidTask()
        {
            var firstTaskId = Guid.NewGuid();
            var invalidId = Guid.NewGuid();

            this.Given(x => x.GivenProjectWithStartingDate(DateTime.UtcNow))
                .And(x => x.AndProjectHasPendingTaskWithId(firstTaskId))
                .When(x => x.WhenIAskForEstimatedStartingDateForTask(invalidId))
                .Then(x => x.ExceptionIsThrownWithMessage($"Invalid task id {invalidId}"))
                .BDDfy();
        }
    }
}