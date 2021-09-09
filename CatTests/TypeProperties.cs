using CatApi.types;
using CatImplementations.typings;
using CatImplementations.typings.primitives;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class TypeProperties
    {
        public ITypeStorage TypeStorage;

        [SetUp]
        public void SetUp()
        {
            TypeStorage = new TypeStorage();
        }

        [Test]
        public void ObjectTypeHave_ToString_Method()
        {
            var objectType = TypeStorage[Primitives.Object];
            objectType.Properties.Should().Contain(p => p.Name == "toString");
        }
    }
}