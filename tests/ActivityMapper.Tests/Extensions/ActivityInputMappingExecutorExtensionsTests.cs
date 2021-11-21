namespace ActivityMapper.Tests.Extensions
{
    using System.Threading.Tasks;
    using AutoFixture.NUnit3;
    using NUnit.Framework;
    using Moq;
    using FluentAssertions;
    using ActivityMapper.Interfaces;
    using ActivityMapper.Extensions;
    using ActivityMapper.Tests.TestModels;

    [TestFixture]
    public class ActivityInputMappingExecutorExtensionsTests
    {
        [Test]
        [AutoData]
        public async Task MapAsync_SingleGeneric_ShouldCallMapAsyncAndReturnResult(FooRequest request, FooInput input)
        {
            // arrange
            var mock = new Mock<IActivityInputMappingExecutor>();
            mock.Setup(m => m.MapAsync(request, typeof(FooInput))).ReturnsAsync(input);

            // act
            var result = await mock.Object.MapAsync<FooInput>(request);

            // assert
            result.Should().Be(input);
            mock.Verify(m => m.MapAsync(request, typeof(FooInput)), Times.Once);
        }

        [Test]
        [AutoData]
        public async Task MapAsync_Multiple_ShouldCallMapAsyncForAllInputsAndReturnNonNullValues(FooRequest request, FooInput inputFoo, BarInput inputBar)
        {
            // arrange
            var mock = new Mock<IActivityInputMappingExecutor>();
            mock.Setup(m => m.MapAsync(request, typeof(FooInput))).ReturnsAsync(inputFoo);
            mock.Setup(m => m.MapAsync(request, typeof(BarInput))).ReturnsAsync(inputBar);
            mock.Setup(m => m.MapAsync(request, typeof(BazInput))).ReturnsAsync(default(IActivityInput));

            // act
            var result = await mock.Object.MapAsync(request, new[] { typeof(FooInput), typeof(BarInput), typeof(BazInput) });

            // assert
            result.Should().HaveCount(2);
            result.Should().Contain(inputFoo);
            result.Should().Contain(inputBar);

            mock.Verify(m => m.MapAsync(request, typeof(FooInput)), Times.Once);
            mock.Verify(m => m.MapAsync(request, typeof(BarInput)), Times.Once);
            mock.Verify(m => m.MapAsync(request, typeof(BazInput)), Times.Once);

        }
    }
}