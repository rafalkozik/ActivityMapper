namespace ActivityMapper.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IActivityInputMappingExecutor
    {
        Task<IActivityInput> MapAsync(object input, Type desiredType);

    }
}
