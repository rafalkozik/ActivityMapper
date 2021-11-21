namespace ActivityMapper.Autofac
{
    using System.Linq;
    using System.Reflection;
    using global::Autofac;
    using ActivityMapper.Interfaces;

    public static class ContainerBuilderExtensions
    {
        public static void RegisterAllActivitiesFromAssembly(this ContainerBuilder builder, Assembly assembly)
        {
            var activityType = typeof(IActivity<>);

            builder.RegisterAssemblyTypes(assembly)
               .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == activityType))
               .AsImplementedInterfaces();
        }

        public static void RegisterAllActivityInputMappersFromAssembly(this ContainerBuilder builder, Assembly assembly)
        {
            var activityType = typeof(IActivityInputMapper);

            builder.RegisterAssemblyTypes(assembly)
               .Where(t => t.IsAssignableTo(activityType))
               .AsImplementedInterfaces();
        }
    }
}
