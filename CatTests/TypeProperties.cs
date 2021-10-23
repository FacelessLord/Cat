using CatApi.bindings;
using CatApi.types;
using CatApi.types.containers;
using CatApi.types.primitives;
using CatDi.di;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class TypeProperties
    {
        public TypeStorage TypeStorage;
        public Kernel Kernel;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Kernel = Main.CreateTestKernel();
            var bindings = Kernel.Resolve<PrimitivesMethodsBindings>();
        }

        [SetUp]
        public void SetUp()
        {
            TypeStorage = Kernel.Resolve<TypeStorage>();
        }

        [Test]
        public void ObjectTypeHave_ToString_Method()
        {
            var objectType = TypeStorage[PrimitivesNames.Object];
            
            objectType.Properties.Should().Contain(p => p.Name == "toString");
        }
    }
}