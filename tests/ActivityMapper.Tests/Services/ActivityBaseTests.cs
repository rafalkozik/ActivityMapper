namespace ActivityMapper.Tests.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture.NUnit3;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ActivityMapper.Services;
    using ActivityMapper.Tests.TestModels;

    [TestFixture]
    public class ActivityBaseTests
    {
        [Test]
        [AutoData]
        public void ExecuteAsync_ShouldThrowIfWrongInputType(BarInput barInput)
        {
            // arrange
            var activityMock = new Mock<ActivityBase<TestActivityIds, FooInput, FooOutput>>();

            // act
            Func<Task> action = async () => await activityMock.Object.ExecuteAsync(barInput);

            // assert
            action.Should().ThrowAsync<ArgumentException>();
        }
    }
}
