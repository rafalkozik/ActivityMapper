namespace ActivityMapper.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ActivityMapper.Models;

    public interface IActivityExecutor<TId> where TId : Enum
    {
        Task<IReadOnlyList<ActivityExecutionResult<TId>>> ExecuteAsync<TInput>(TInput input) where TInput : IActivityInput;
        
        ISet<Type> SupportedInputs { get; }

        Type GetActivityInputType(TId activityId);
    }
}