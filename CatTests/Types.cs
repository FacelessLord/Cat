using System.Collections.Generic;
using Cat;
using Cat.data;
using Cat.data.types;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class Types
    {
        public ITypeStorage TypeStorage;

        [SetUp]
        public void SetUp()
        {
            TypeStorage = new NonCachingTypeStorage();
        }

        [Test]
        public void EmptyType_IsAssignableTo_Itself()
        {
            var a = TypeStorage.GetOrCreateType("A", "test.A", new List<IDataMember>());

            a.IsAssignableTo(a).Should().BeTrue();
        }

        [Test]
        public void EmptyType_IsAssignableTo_AnotherEmptyType()
        {
            var a = TypeStorage.GetOrCreateType("A", "test.A", new List<IDataMember>());
            var b = TypeStorage.GetOrCreateType("B", "test.B", new List<IDataMember>());

            a.IsAssignableTo(b).Should().BeTrue();
        }

        [Test]
        public void NonEmptyType_IsAssignableTo_EmptyType()
        {
            var any = TypeStorage.GetOrCreateType("Any", "system.Any", new List<IDataMember>());
            var anyA = new DataMember(any, "anyA");
            var anyB = new DataMember(any, "anyB");
            var a = TypeStorage.GetOrCreateType("A", "test.A", new List<IDataMember> { anyA, anyB });
            var b = TypeStorage.GetOrCreateType("B", "test.B", new List<IDataMember>());

            a.IsAssignableTo(b).Should().BeTrue();
        }

        [Test]
        public void EmptyType_IsAssignableFrom_NonEmptyType()
        {
            var any = TypeStorage.GetOrCreateType("Any", "system.Any", new List<IDataMember>());
            var anyA = new DataMember(any, "anyA");
            var anyB = new DataMember(any, "anyB");
            var a = TypeStorage.GetOrCreateType("A", "test.A", new List<IDataMember> { anyA, anyB });
            var b = TypeStorage.GetOrCreateType("B", "test.B", new List<IDataMember>());

            b.IsAssignableFrom(a).Should().BeTrue();
        }

        [Test]
        public void Simple_NonEmptyType_IsAssignableFrom_NonEmptyType()
        {
            var any = TypeStorage.GetOrCreateType("Any", "system.Any", new List<IDataMember>());
            var anyA = new DataMember(any, "anyA");
            var anyB = new DataMember(any, "anyB");
            var anyC = new DataMember(any, "anyA");
            var anyD = new DataMember(any, "anyB");
            var a = TypeStorage.GetOrCreateType("A", "test.A", new List<IDataMember> { anyA, anyB });
            var b = TypeStorage.GetOrCreateType("B", "test.B", new List<IDataMember> { anyC, anyD });

            b.IsAssignableFrom(a).Should().BeTrue();
        }

        [Test]
        public void Nested_NonEmptyType_IsAssignableFrom_NonEmptyType()
        {
            var any = TypeStorage.GetOrCreateType("Any", "system.Any", new List<IDataMember>());
            var anyBc = new DataMember(any, "anyB");
            var anyCc = new DataMember(any, "anyA");
            var c = TypeStorage.GetOrCreateType("C", "test.C", new List<IDataMember> { anyBc, anyCc });
            var anyBd = new DataMember(any, "anyB");
            var anyCd = new DataMember(any, "anyA");
            var d = TypeStorage.GetOrCreateType("D", "test.D", new List<IDataMember> { anyBd, anyCd });
            var anyA = new DataMember(c, "anyC");
            var anyD = new DataMember(d, "anyC");
            var a = TypeStorage.GetOrCreateType("A", "test.A", new List<IDataMember> { anyA });
            var b = TypeStorage.GetOrCreateType("B", "test.B", new List<IDataMember> { anyD });

            b.IsAssignableFrom(a).Should().BeTrue();
        }
    }
}