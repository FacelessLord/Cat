using Cat.data.properties;
using Cat.data.types;
using Cat.data.types.primitives;
using Cat.data.types.primitives.@object;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class Types
    {
        public TypeStorage types;

        [SetUp]
        public void SetUp()
        {
            types = new TypeStorage();
        }

        [Test]
        public void EmptyType_IsAssignableTo_Itself()
        {
            var a = new AnyType(types);

            a.IsAssignableTo(a).Should().BeTrue();
        }

        [Test]
        public void EmptyType_IsAssignableTo_AnotherEmptyType()
        {
            var a = new AnyType(types);
            var b = new AnyType(types);

            a.IsAssignableTo(b).Should().BeTrue();
        }

        [Test]
        public void NonEmptyType_IsAssignableTo_EmptyType()
        {
            var a = new ObjectType(types);
            var b = new AnyType(types);

            a.IsAssignableTo(b).Should().BeTrue();
        }

        [Test]
        public void EmptyType_IsAssignableFrom_NonEmptyType()
        {
            var a = new ObjectType(types);
            var b = new AnyType(types);

            b.IsAssignableFrom(a).Should().BeTrue();
        }

        [Test]
        public void Simple_NonEmptyType_IsAssignableFrom_NonEmptyType()
        {
            var a = new ObjectType(types);
            var b = new ObjectType(types);

            b.IsAssignableFrom(a).Should().BeTrue();
        }
    }
}