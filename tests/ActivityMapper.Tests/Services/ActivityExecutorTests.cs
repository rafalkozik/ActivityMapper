namespace ActivityMapper.Tests.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture.NUnit3;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ActivityMapper.Interfaces;
    using ActivityMapper.Services;
    using ActivityMapper.Tests.TestModels;

    [TestFixture]
    public class ActivityExecutorTests
    {
        [Test]
        [AutoData]
        public async Task NoActivities_ShouldNotThrow(FooInput input)
        {
            // arrange
            var sut = new ActivityExecutor<TestActivityIds>(activities: null);

            // act
            var result = await sut.ExecuteAsync(input);

            // assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        [AutoData]
        public async Task RegisteredActivities_ShouldDoExecution(FooInput input, FooOutput output)
        {
            // arrange
            var activityMock = new Mock<ActivityBase<TestActivityIds, FooInput, FooOutput>>();

            activityMock.SetupGet(m => m.Id).Returns(TestActivityIds.Foo);
            activityMock.Setup(m => m.ExecuteAsync(It.IsAny<FooInput>())).ReturnsAsync(output);

            var sut = new ActivityExecutor<TestActivityIds>(new [] { activityMock.Object });

            // act
            var result = await sut.ExecuteAsync(input);

            // assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(r => r.ActivityId == TestActivityIds.Foo
                && r.ActivityInput == input
                && r.ActivityOutput == output);

            activityMock.Verify(m => m.ExecuteAsync(input), Times.Once());
        }

        [Test]
        [AutoData]
        public async Task RegisteredActivities_ShouldDoExecution_MultipleActivities(FooInput input, FooOutput fooOutput, BarOutput barOutput)
        {
            // arrange
            var activityMockA = new Mock<ActivityBase<TestActivityIds, FooInput, FooOutput>>();
            var activityMockB = new Mock<ActivityBase<TestActivityIds, FooInput, BarOutput>>();
            var activityMockC = new Mock<ActivityBase<TestActivityIds, BarInput, BarOutput>>();

            activityMockA.SetupGet(m => m.Id).Returns(TestActivityIds.Foo);
            activityMockA.Setup(m => m.ExecuteAsync(It.IsAny<FooInput>())).ReturnsAsync(fooOutput);

            activityMockB.SetupGet(m => m.Id).Returns(TestActivityIds.Bar);
            activityMockB.Setup(m => m.ExecuteAsync(It.IsAny<FooInput>())).ReturnsAsync(barOutput);

            var sut = new ActivityExecutor<TestActivityIds>(
                activities: new IActivity<TestActivityIds>[] { activityMockA.Object, activityMockB.Object, activityMockC.Object });

            // act
            var result = await sut.ExecuteAsync(input);

            // assert
            result.Should().HaveCount(2);
            
            result.Should().ContainSingle(r => r.ActivityId == TestActivityIds.Foo
                && r.ActivityInput == input
                && r.ActivityOutput == fooOutput);

            result.Should().ContainSingle(r => r.ActivityId == TestActivityIds.Bar
                && r.ActivityInput == input
                && r.ActivityOutput == barOutput);

            activityMockA.Verify(m => m.ExecuteAsync(input), Times.Once());
            activityMockB.Verify(m => m.ExecuteAsync(input), Times.Once());
            activityMockC.Verify(m => m.ExecuteAsync(It.IsAny<BarInput>()), Times.Never());
        }

        [Test]
        [AutoData]
        public void NoActivities_SupportedInputsShouldBeEmpty(FooInput input)
        {
            // arrange
            var sut = new ActivityExecutor<TestActivityIds>(activities: null);

            // assert
            sut.SupportedInputs.Should().BeEmpty();
        }

        [Test]
        [AutoData]
        public void RegisteredActivities_SupportedInputsShouldHaveAllInputTypes(FooInput input, FooOutput fooOutput, BarOutput barOutput)
        {
            // arrange
            var activityMockA = new Mock<ActivityBase<TestActivityIds, FooInput, FooOutput>>();
            var activityMockB = new Mock<ActivityBase<TestActivityIds, FooInput, BarOutput>>();
            var activityMockC = new Mock<ActivityBase<TestActivityIds, BarInput, BarOutput>>();

            var sut = new ActivityExecutor<TestActivityIds>(
                activities: new IActivity<TestActivityIds>[] { activityMockA.Object, activityMockB.Object, activityMockC.Object });

            // assert
            sut.SupportedInputs.Should().BeEquivalentTo(new[] { typeof(FooInput), typeof(BarInput) });
        }

        [Test]
        [AutoData]
        public void RegisteredActivities_GetActivityInputType_ReturnValueIfFound()
        {
            // arrange
            var activityMockA = new Mock<ActivityBase<TestActivityIds, FooInput, FooOutput>>();
            var activityMockB = new Mock<ActivityBase<TestActivityIds, BarInput, BarOutput>>();

            activityMockA.SetupGet(m => m.Id).Returns(TestActivityIds.Foo);
            activityMockB.SetupGet(m => m.Id).Returns(TestActivityIds.Bar);


            var sut = new ActivityExecutor<TestActivityIds>(
                activities: new IActivity<TestActivityIds>[] { activityMockA.Object, activityMockB.Object });

            // assert
            sut.GetActivityInputType(TestActivityIds.Foo).Should().Be(activityMockA.Object.InOutTypes.In);
            sut.GetActivityInputType(TestActivityIds.Bar).Should().Be(activityMockB.Object.InOutTypes.In);
        }

        [Test]
        [AutoData]
        public void RegisteredActivities_GetActivityInputType_ThrowIfNotFound()
        {
            // arrange
            var activityMockA = new Mock<ActivityBase<TestActivityIds, FooInput, FooOutput>>();
            activityMockA.SetupGet(m => m.Id).Returns(TestActivityIds.Foo);


            var sut = new ActivityExecutor<TestActivityIds>(
                activities: new IActivity<TestActivityIds>[] { activityMockA.Object });

            Action action = () => sut.GetActivityInputType(TestActivityIds.Bar);

            // assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        [AutoData]
        public void RegisteredActivities_GetActivityInputType_ThrowIfDuplicateFound()
        {
            // arrange
            var activityMockA = new Mock<ActivityBase<TestActivityIds, FooInput, FooOutput>>();
            var activityMockB = new Mock<ActivityBase<TestActivityIds, BarInput, BarOutput>>();

            activityMockA.SetupGet(m => m.Id).Returns(TestActivityIds.Foo);
            activityMockB.SetupGet(m => m.Id).Returns(TestActivityIds.Foo);


            var sut = new ActivityExecutor<TestActivityIds>(
                activities: new IActivity<TestActivityIds>[] { activityMockA.Object, activityMockB.Object });

            Action action = () => sut.GetActivityInputType(TestActivityIds.Foo);

            // assert
            action.Should().Throw<InvalidOperationException>();
        }

    }
}