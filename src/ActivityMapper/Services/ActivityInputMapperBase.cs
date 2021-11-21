namespace ActivityMapper.Services
{
    using System;
    using System.Threading.Tasks;
    using ActivityMapper.Interfaces;
    using ActivityMapper.Models;

    public abstract class ActivityInputMapperBase<TInput, TOutput> : IActivityInputMapper
        where TInput : class
        where TOutput : IActivityInput
    {
        public InOutTypes InOutTypes { get; } = new InOutTypes { In = typeof(TInput), Out = typeof(TOutput) };

        public async Task<IActivityInput> MapAsync(object input)
        {
            if (input is not TInput)
            {
                throw new ArgumentException($"Expected {typeof(TInput)}", nameof(input));
            }

            return await MapAsync(input as TInput);
        }

        public abstract Task<TOutput> MapAsync(TInput input);
    }
}
