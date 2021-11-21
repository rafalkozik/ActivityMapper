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
    public class ActivityInputMapperBaseTests
    {
        [Test]
        [AutoData]
        public void ExecuteAsync_ShouldThrowIfWrongInputType(BarRequest barRequest)
        {
            // arrange
            var activityMock = new Mock<ActivityInputMapperBase<FooRequest, FooInput>>();

            // act
            Func<Task> action = async () => await activityMock.Object.MapAsync(barRequest);

            // assert
            action.Should().ThrowAsync<ArgumentException>();
        }
    }
}
