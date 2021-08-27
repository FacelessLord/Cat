using System.Collections.Generic;
using Cat;
using Cat.data;
using Cat.data.properties;
using Cat.data.properties.api;
using Cat.data.types;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class Types
    {
        public PropertyMeta plainMeta = new PropertyMeta();

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void EmptyType_IsAssignableTo_Itself()
        {
            var a = new DataType("A", "test.A", new List<IDataProperty>());

            a.IsAssignableTo(a).Should().BeTrue();
        }

        [Test]
        public void EmptyType_IsAssignableTo_AnotherEmptyType()
        {
            var a = new DataType("A", "test.A", new List<IDataProperty>());
            var b = new DataType("B", "test.B", new List<IDataProperty>());

            a.IsAssignableTo(b).Should().BeTrue();
        }

        [Test]
        public void NonEmptyType_IsAssignableTo_EmptyType()
        {
            var any = new DataType("Any", "system.Any", new List<IDataProperty>());
            var anyA = new DataProperty(any, plainMeta, "anyA");
            var anyB = new DataProperty(any, plainMeta, "anyB");
            var a = new DataType("A", "test.A", new List<IDataProperty> { anyA, anyB });
            var b = new DataType("B", "test.B", new List<IDataProperty>());

            a.IsAssignableTo(b).Should().BeTrue();
        }

        [Test]
        public void EmptyType_IsAssignableFrom_NonEmptyType()
        {
            var any = new DataType("Any", "system.Any", new List<IDataProperty>());
            var anyA = new DataProperty(any, plainMeta, "anyA");
            var anyB = new DataProperty(any, plainMeta, "anyB");
            var a = new DataType("A", "test.A", new List<IDataProperty> { anyA, anyB });
            var b = new DataType("B", "test.B", new List<IDataProperty>());

            b.IsAssignableFrom(a).Should().BeTrue();
        }

        [Test]
        public void Simple_NonEmptyType_IsAssignableFrom_NonEmptyType()
        {
            var any = new DataType("Any", "system.Any", new List<IDataProperty>());
            var anyA = new DataProperty(any, plainMeta, "anyA");
            var anyB = new DataProperty(any, plainMeta, "anyB");
            var anyC = new DataProperty(any, plainMeta, "anyA");
            var anyD = new DataProperty(any, plainMeta, "anyB");
            var a = new DataType("A", "test.A", new List<IDataProperty> { anyA, anyB });
            var b = new DataType("B", "test.B", new List<IDataProperty> { anyC, anyD });

            b.IsAssignableFrom(a).Should().BeTrue();
        }

        [Test]
        public void Nested_NonEmptyType_IsAssignableFrom_NonEmptyType()
        {
            var any = new DataType("Any", "system.Any", new List<IDataProperty>());
            var anyBc = new DataProperty(any, plainMeta, "anyB");
            var anyCc = new DataProperty(any, plainMeta, "anyA");
            var c = new DataType("C", "test.C", new List<IDataProperty> { anyBc, anyCc });
            var anyBd = new DataProperty(any, plainMeta, "anyB");
            var anyCd = new DataProperty(any, plainMeta, "anyA");
            var d = new DataType("D", "test.D", new List<IDataProperty> { anyBd, anyCd });
            var anyA = new DataProperty(c, plainMeta, "anyC");
            var anyD = new DataProperty(d, plainMeta, "anyC");
            var a = new DataType("A", "test.A", new List<IDataProperty> { anyA });
            var b = new DataType("B", "test.B", new List<IDataProperty> { anyD });

            b.IsAssignableFrom(a).Should().BeTrue();
        }
    }
}