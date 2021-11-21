namespace ActivityMapper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ActivityMapper.Interfaces;
    using ActivityMapper.Models;

    internal class ActivityExecutor<TId> : IActivityExecutor<TId> 
        where TId : Enum
    {
        private readonly IActivity<TId>[] activities;
        private readonly Dictionary<Type, IActivity<TId>[]> activitiesByInputType;

        public ActivityExecutor(IActivity<TId>[] activities)
        {
            this.activities = activities ?? Array.Empty<IActivity<TId>>();

            this.activitiesByInputType = this.activities.GroupBy(a => a.InOutTypes.In).ToDictionary(g => g.Key, g => g.ToArray());
            this.SupportedInputs = this.activities.Select(a => a.InOutTypes.In).ToHashSet();
        }

        public ISet<Type> SupportedInputs { get; private set; }

        public async Task<IReadOnlyList<ActivityExecutionResult<TId>>> ExecuteAsync<TInput>(TInput input)
            where TInput : IActivityInput
        {
            var result = new List<ActivityExecutionResult<TId>>();
            var inputType = input.GetType();

            if (!activitiesByInputType.TryGetValue(inputType, out var matchingActivities))
            {
                return result;
            }

            foreach (var activity in matchingActivities)
            {
                var actionOutput = await ExecuteDirectAsync(input, activity);
                result.Add(actionOutput);
            }

            return result;
        }

        public Type GetActivityInputType(TId activityId)
        {
            return this.activities.Single(a => a.Id.Equals(activityId)).InOutTypes.In;
        }

        internal static async Task<ActivityExecutionResult<TId>> ExecuteDirectAsync(IActivityInput input, IActivity<TId> activity)
        {
            var activityOutput = await activity.ExecuteAsync(input);

            return new ActivityExecutionResult<TId>
            {
                ActivityId = activity.Id,
                ActivityInput = input,
                ActivityOutput = activityOutput,
            };
        }
    }
}
