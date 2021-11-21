namespace ActivityMapper.Demo
{
    using System;
    using System.Threading.Tasks;
    using global::Autofac;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using ActivityMapper.Interfaces;
    using ActivityMapper.Autofac;
    using ActivityMapper.Extensions;
    using ActivityMapper.Demo.Activities;
    using ActivityMapper.Demo.Models;

    class Program
    {
        static async Task Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<ActivityMapperModule>();
            containerBuilder.RegisterAllActivitiesFromAssembly(typeof(Program).Assembly);
            containerBuilder.RegisterAllActivityInputMappersFromAssembly(typeof(Program).Assembly);

            using (var container = containerBuilder.Build())
            {
                var activityExecutor = container.Resolve<IActivityExecutor<Ids>>();
                var activityInputMappingExecutor = container.Resolve<IActivityInputMappingExecutor>();

                await DemoDirectExecutionAsync(activityExecutor);
                await DemoMappedExecutionAsync(activityExecutor, activityInputMappingExecutor);
                await DemoJsonExecutionAsync(activityExecutor);
            }
        }

        private static async Task DemoDirectExecutionAsync(IActivityExecutor<Ids> executor)
        {
            Console.WriteLine("================ DemoDirectExecutionAsync ================");
            
            var input = new SampleActivityInput
            {
                Name = "test",
            };

            var result = await executor.ExecuteAsync(input);

            ShowResults(result);
        }

        private static async Task DemoMappedExecutionAsync(IActivityExecutor<Ids> executor, IActivityInputMappingExecutor mapper)
        {
            Console.WriteLine("============= DemoDirectExecutionAsync (auto) ============");

            var inputFoo = new FooRequest
            {
                Foo = "Foo"
            };

            var mappedInputs = await mapper.MapAsync(inputFoo, executor.SupportedInputs);

            foreach (var input in mappedInputs)
            {
                var result = await executor.ExecuteAsync(input);
                ShowResults(result);
            }

            Console.WriteLine("========= DemoDirectExecutionAsync (target type) =========");

            var mappedInput = await mapper.MapAsync<SampleActivityInput>(inputFoo);

            if (mappedInput != null)
            {
                var result = await executor.ExecuteAsync(mappedInput);
                ShowResults(result);
            }
        }

        private static async Task DemoJsonExecutionAsync(IActivityExecutor<Ids> executor)
        {
            Console.WriteLine("================= DemoJsonExecutionAsync =================");

            var json = JObject.Parse("{ \"Name\" : \"Bar\" }");

            var desiredType = executor.GetActivityInputType(Ids.SampleA);
            var deserializedJson = json.ToObject(desiredType) as IActivityInput;

            var result = await executor.ExecuteAsync(deserializedJson);
            ShowResults(result);
        }

        private static void ShowResults<TResult>(TResult result)
        {
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Console.WriteLine();
        }
    }
}
