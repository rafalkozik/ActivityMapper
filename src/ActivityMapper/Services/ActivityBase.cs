namespace ActivityMapper.Services
{
    using System;
    using System.Threading.Tasks;
    using ActivityMapper.Interfaces;
    using ActivityMapper.Models;

    public abstract class ActivityBase<TId, TInput, TOutput> : IActivity<TId>
        where TId : Enum
        where TInput : class, IActivityInput
        where TOutput : class, IActivityOutput
    {
        public InOutTypes InOutTypes { get; } = new InOutTypes { In = typeof(TInput), Out = typeof(TOutput) };

        public async Task<IActivityOutput> ExecuteAsync(IActivityInput input)
        {
            if (input is not TInput)
            {
                throw new ArgumentException($"Expected {typeof(TInput)}", nameof(input));
            }

            return await ExecuteAsync(input as TInput);
        }

        public abstract TId Id { get; }

        public abstract Task<TOutput> ExecuteAsync(TInput input);
    }
}
