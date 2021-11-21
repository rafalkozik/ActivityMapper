namespace ActivityMapper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ActivityMapper.Interfaces;

    internal class ActivityInputMappingExecutor : IActivityInputMappingExecutor
    {
        private readonly IActivityInputMapper[] inputMappers;
        private readonly Dictionary<(Type input, Type output), IActivityInputMapper> inputMapperByTypes;
        private readonly Dictionary<Type, List<Type>> mappingSourcesPerType;

        public ActivityInputMappingExecutor(IActivityInputMapper[] inputMappers)
        {
            this.inputMappers = inputMappers ?? Array.Empty<IActivityInputMapper>();
            this.inputMapperByTypes = this.inputMappers.ToDictionary(m => (m.InOutTypes.In, m.InOutTypes.Out));
            this.mappingSourcesPerType = new Dictionary<Type, List<Type>>();
        }

        public async Task<IActivityInput> MapAsync(object input, Type desiredType)
        {
            var inputType = input.GetType();

            foreach (var sourceType in GetTypeMappingSources(inputType))
            {
                if (this.inputMapperByTypes.TryGetValue((sourceType, desiredType), out var inputMapper))
                {
                    return await inputMapper.MapAsync(input);
                }
            }

            return null;
        }

        private List<Type> GetTypeMappingSources(Type inputType)
        {
            if (this.mappingSourcesPerType.TryGetValue(inputType, out var mappingSources))
            {
                return mappingSources;
            }

            mappingSources = new List<Type>() { inputType };
            mappingSources.AddRange(inputType.GetInterfaces());

            var baseType = inputType.BaseType;

            while (baseType != typeof(Object) && baseType != null)
            {
                mappingSources.Add(baseType);
                baseType = baseType.BaseType;
            }

            this.mappingSourcesPerType.Add(inputType, mappingSources);

            return mappingSources;
        }
    }
}
