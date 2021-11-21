namespace ActivityMapper.Tests.TestModels
{
    public class FooRequest : FooRequestBase
    {
        public string Foo { get; set; }
    }

    public class FooRequestBase : FooRequestBaseBase, IFooRequestBase
    { 
    }

    public class FooRequestBaseBase
    {
    }

    public interface IFooRequestBase
    {
    }
}
