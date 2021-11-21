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
    public class ActivityInputMappingExecutorTests
    {
        [Test]
        [AutoData]
        public async Task NoMappings_ShouldReturnNull(FooRequest request)
        {
            // arrange
            var sut = new ActivityInputMappingExecutor(inputMappers: null);

            // act
            var result = await sut.MapAsync(request, typeof(FooInput));

            // assert
            result.Should().BeNull();
        }

        [Test]
        [AutoData]
        public async Task RegisteredMappings_NotFound_ShouldReturnNull(FooRequest request)
        {
            // arrange
            var inputMapper = new Mock<ActivityInputMapperBase<BarRequest, FooInput>>();
            var sut = new ActivityInputMappingExecutor(new[] { inputMapper.Object });

            // act
            var result = await sut.MapAsync(request, typeof(FooInput));

            // assert
            result.Should().BeNull();
        }

        [Test]
        [AutoData]
        public async Task RegisteredMappings_Found_ShouldReturnNull(FooRequest request, FooInput input)
        {
            // arrange
            var inputMapper = new Mock<ActivityInputMapperBase<FooRequest, FooInput>>();
            inputMapper.Setup(m => m.MapAsync(request)).ReturnsAsync(input);

            var sut = new ActivityInputMappingExecutor(new[] { inputMapper.Object });

            // act
            var result = await sut.MapAsync(request, typeof(FooInput));

            // assert
            result.Should().Be(input);
        }

        [Test]
        [AutoData]
        public void RegisteredMappings_ShouldNotAllowDuplicatedMappings()
        {
            // arrange
            var inputMapperA = new Mock<ActivityInputMapperBase<FooRequest, FooInput>>();
            var inputMapperB = new Mock<ActivityInputMapperBase<FooRequest, FooInput>>();

            // act
            Action action = () => new ActivityInputMappingExecutor(new[] { inputMapperA.Object, inputMapperB.Object });

            // act
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        [AutoData]
        public async Task RegisteredMappings_Found_ShouldUseMappingByInterface(FooRequest request, FooInput input)
        {
            // arrange
            var inputMapper = new Mock<ActivityInputMapperBase<IFooRequestBase, FooInput>>();
            inputMapper.Setup(m => m.MapAsync(request)).ReturnsAsync(input);

            var sut = new ActivityInputMappingExecutor(new[] { inputMapper.Object });

            // act
            var result = await sut.MapAsync(request, typeof(FooInput));

            // assert
            result.Should().Be(input);
        }

        [Test]
        [AutoData]
        public async Task RegisteredMappings_Found_ShouldUseMappingByBaseClass(FooRequest request, FooInput input)
        {
            // arrange
            var inputMapper = new Mock<ActivityInputMapperBase<FooRequestBaseBase, FooInput>>();
            inputMapper.Setup(m => m.MapAsync(request)).ReturnsAsync(input);

            var sut = new ActivityInputMappingExecutor(new[] { inputMapper.Object });

            // act
            var result = await sut.MapAsync(request, typeof(FooInput));

            // assert
            result.Should().Be(input);
        }

        [Test]
        [AutoData]
        public async Task RegisteredMappings_FoundMultiple_ShouldPreferDirectMapping(FooRequest request, FooInput inputA, FooInput inputB, FooInput inputC)
        {
            // arrange
            var inputMapperA = new Mock<ActivityInputMapperBase<IFooRequestBase, FooInput>>();
            inputMapperA.Setup(m => m.MapAsync(request)).ReturnsAsync(inputA);

            var inputMapperB = new Mock<ActivityInputMapperBase<FooRequestBase, FooInput>>();
            inputMapperB.Setup(m => m.MapAsync(request)).ReturnsAsync(inputB);

            var inputMapperC = new Mock<ActivityInputMapperBase<FooRequest, FooInput>>();
            inputMapperC.Setup(m => m.MapAsync(request)).ReturnsAsync(inputC);

            var expectedResult = inputC;

            async Task RunTest(params IActivityInputMapper[] mappers)
            {
                var sut = new ActivityInputMappingExecutor(mappers);
                // act
                var result = await sut.MapAsync(request, typeof(FooInput));

                // assert
                result.Should().Be(expectedResult);
            }

            await RunTest(inputMapperA.Object, inputMapperB.Object, inputMapperC.Object);
            await RunTest(inputMapperA.Object, inputMapperC.Object, inputMapperB.Object);
            await RunTest(inputMapperB.Object, inputMapperA.Object, inputMapperC.Object);
            await RunTest(inputMapperB.Object, inputMapperC.Object, inputMapperA.Object);
            await RunTest(inputMapperC.Object, inputMapperA.Object, inputMapperB.Object);
            await RunTest(inputMapperC.Object, inputMapperB.Object, inputMapperA.Object);
        }
    }
}
