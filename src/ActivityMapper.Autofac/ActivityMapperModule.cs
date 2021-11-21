namespace ActivityMapper.Autofac
{
    using global::Autofac;
    using ActivityMapper.Interfaces;
    using ActivityMapper.Services;

    public class ActivityMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ActivityExecutor<>)).As(typeof(IActivityExecutor<>)).SingleInstance();
            builder.RegisterType<ActivityInputMappingExecutor>().As<IActivityInputMappingExecutor>().SingleInstance();
        }
    }
}
