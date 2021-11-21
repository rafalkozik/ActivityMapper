namespace ActivityMapper.Demo.Activities
{
    using System.Threading.Tasks;
    using ActivityMapper.Services;
    using ActivityMapper.Demo.Models;

    public class SampleActivityA : ActivityBase<Ids, SampleActivityInput, SampleActivityOutput>
    {
        public override Ids Id => Ids.SampleA;

        public override Task<SampleActivityOutput> ExecuteAsync(SampleActivityInput input)
        {
            return Task.FromResult(new SampleActivityOutput
            {
                Result = $"{input.Name} (in SampleA)",
            });
        }
    }
}
