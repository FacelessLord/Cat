﻿using Cat.data.objects.primitives;
using Cat.data.types;
using Cat.data.types.primitives;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class Objects
    {
        public ITypeStorage TypeStorage;

        [SetUp]
        public void SetUp()
        {
            TypeStorage = new TypeStorage();
        }

        [Test]
        public void Creates_Object_FromType()
        {
            var objectType = TypeStorage[Primitives.Object];
            var obj = objectType.CreateInstance();
            obj.Should().BeOfType<DataObject>();
        }
    }
}