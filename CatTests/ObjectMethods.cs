using CatApi.bindings;
using CatApi.bindings.objectBindings;
using CatApi.objects.primitives;
using CatApi.types;
using CatApi.types.containers;
using CatApi.types.primitives;
using CatDi.di;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class ObjectMethods
    {
        public TypeStorage TypeStorage;
        public Kernel Kernel;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Kernel = Main.CreateTestKernel();
        }
        [SetUp]
        public void SetUp()
        {
            TypeStorage = Kernel.Resolve<TypeStorage>();
            var bindings = Kernel.Resolve<PrimitivesMethodsBindings>();
        }

        [Test]
        public void Casts_Object_ToString()
        {
            var obj = new DataObject(Primitives.Object);
            TypeStorage[Primitives.Object].PopulateObject(obj);

            obj.Properties.Should().ContainKey("toString");
            obj.GetProperty("toString").Should().BeOfType<BoundMethod>();
            var toStringResult = obj.CallProperty("toString", new[] { obj });
            toStringResult.Should().BeOfType<StringDataObject>();
            (toStringResult as StringDataObject)!.Value.Should().Be("system.Object {toString: (system.Object) -> system.String}");
        }
    }
}