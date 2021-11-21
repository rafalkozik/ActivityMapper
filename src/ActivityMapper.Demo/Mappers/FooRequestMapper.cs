namespace ActivityMapper.Demo.Mappers
{
    using System.Threading.Tasks;
    using ActivityMapper.Services;
    using ActivityMapper.Demo.Models;

    public class FooRequestMapper : ActivityInputMapperBase<FooRequest, SampleActivityInput>
    {
        public override Task<SampleActivityInput> MapAsync(FooRequest input)
        {
            return Task.FromResult(new SampleActivityInput
            {
                Name = input.Foo,
            });
        }
    }
}
