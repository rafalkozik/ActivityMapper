namespace ActivityMapper.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ActivityMapper.Interfaces;

    public static class ActivityInputMappingExecutorExtensions
    {
        public static async Task<IReadOnlyList<IActivityInput>> MapAsync<TSource>(
            this IActivityInputMappingExecutor mappingExecutor,
            TSource input,
            IEnumerable<Type> desiredTypes)
            where TSource : class
        {
            var result = new List<IActivityInput>();

            foreach (var desiredType in desiredTypes)
            {
                var mappedInput = await mappingExecutor.MapAsync(input, desiredType);

                if (mappedInput != null)
                {
                    result.Add(mappedInput);
                }
            }

            return result;
        }

        public static async Task<TOutput> MapAsync<TOutput>(
            this IActivityInputMappingExecutor mappingExecutor,
            object input) 
            where TOutput : class, IActivityInput
        {
            var mappedInput = await mappingExecutor.MapAsync(input, typeof(TOutput));
            return mappedInput as TOutput;
        }

    }
}