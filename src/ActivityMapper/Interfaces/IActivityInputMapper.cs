namespace ActivityMapper.Interfaces
{
    using System.Threading.Tasks;
    using ActivityMapper.Models;

    public interface IActivityInputMapper
    {
        InOutTypes InOutTypes { get; }

        Task<IActivityInput> MapAsync(object input);
    }
}
