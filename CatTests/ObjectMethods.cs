using Cat.data.objects.primitives;
using Cat.data.types;
using Cat.data.types.primitives;
using Cat.data.types.primitives.@object;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class ObjectMethods
    {
        public ITypeStorage TypeStorage;

        [SetUp]
        public void SetUp()
        {
            TypeStorage = new TypeStorage();
        }

        [Test]
        public void Casts_Object_ToString()
        {
            var objectType = TypeStorage[Primitives.Object];
            var obj = objectType.CreateInstance();
            
            obj.Properties.Should().ContainKey("toString");
            obj.GetProperty("toString").Should().BeOfType<ToStringMethod>();
            var toStringResult = obj.CallProperty("toString", new []{obj});
            toStringResult.Should().BeOfType<StringDataObject>();
            (toStringResult as StringDataObject)!.Value.Should().Be("Object {toString : (Object) -> String}");
        }
    }
}