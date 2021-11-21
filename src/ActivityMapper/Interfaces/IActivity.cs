namespace ActivityMapper.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using ActivityMapper.Models;

    public interface IActivity<TId>
        where TId : Enum
    {
        TId Id { get; }

        InOutTypes InOutTypes { get; }

        Task<IActivityOutput> ExecuteAsync(IActivityInput input);
    }
}
