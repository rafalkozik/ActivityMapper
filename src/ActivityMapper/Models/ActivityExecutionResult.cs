namespace ActivityMapper.Models
{
    using System;
    using ActivityMapper.Interfaces;

    public class ActivityExecutionResult<TId>
        where TId : Enum
    {
        public TId ActivityId { get; set; }

        public IActivityInput ActivityInput { get; set; }

        public IActivityOutput ActivityOutput { get; set; }
    }
}
