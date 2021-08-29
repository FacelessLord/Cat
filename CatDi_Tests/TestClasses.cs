namespace CatDi_Tests
{
    public class EmptyConstructorClass
    {
    }

    public class OneDependencyClass
    {
        public EmptyConstructorClass EmptyConstructorClass { get; }

        public OneDependencyClass(EmptyConstructorClass emptyConstructorClass)
        {
            EmptyConstructorClass = emptyConstructorClass;
        }
    }

    public class ManyDependencyClass
    {
        public EmptyConstructorClass EmptyConstructorClass { get; }
        public EmptyConstructorClass EmptyConstructorClass2 { get; }
        public OneDependencyClass OneDependencyClass { get; }
        public OneDependencyClass OneDependencyClass2 { get; }

        public ManyDependencyClass(EmptyConstructorClass emptyConstructorClass,
            EmptyConstructorClass emptyConstructorClass2, OneDependencyClass oneDependencyClass,
            OneDependencyClass oneDependencyClass2)
        {
            EmptyConstructorClass = emptyConstructorClass;
            EmptyConstructorClass2 = emptyConstructorClass2;
            OneDependencyClass = oneDependencyClass;
            OneDependencyClass2 = oneDependencyClass2;
        }
    }

    public class CyclicDependencyClassA
    {
        public CyclicDependencyClassB CyclicDependencyClassB { get; }

        public CyclicDependencyClassA(CyclicDependencyClassB cyclicDependencyClassB)
        {
            CyclicDependencyClassB = cyclicDependencyClassB;
        }
    }

    public class CyclicDependencyClassB
    {
        public CyclicDependencyClassA CyclicDependencyClassA { get; }

        public CyclicDependencyClassB(CyclicDependencyClassA cyclicDependencyClassA)
        {
            CyclicDependencyClassA = cyclicDependencyClassA;
        }
    }

    public interface ITestClass
    {
    }

    public class TestClass : ITestClass
    {
    }

    public class TestDependentClass
    {
        public ITestClass TestClass { get; }

        public TestDependentClass(ITestClass testClass)
        {
            TestClass = testClass;
        }
    }
}