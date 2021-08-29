using System;
using System.Linq;
using CatDi.di;
using FluentAssertions;
using NUnit.Framework;

namespace CatDi_Tests
{
    [TestFixture]
    public class Tests
    {
        public Kernel Kernel;

        [SetUp]
        public void Setup()
        {
            Kernel = new Kernel();
        }

        [Test]
        public void RegistersDependency()
        {
            Kernel.Register<EmptyConstructorClass>().As<EmptyConstructorClass>();

            Kernel.Factories.Should().ContainKey(typeof(EmptyConstructorClass));
            Kernel.Factories[typeof(EmptyConstructorClass)].Single().Should().BeOfType<Factory>();
        }

        [Test]
        public void RegistersSingletonDependency()
        {
            Kernel.Register<EmptyConstructorClass>().AsSingleton<EmptyConstructorClass>();

            Kernel.Factories.Should().ContainKey(typeof(EmptyConstructorClass));
            Kernel.Factories[typeof(EmptyConstructorClass)].Single().Should().BeOfType<SingletonFactory>();
        }

        [Test]
        public void RegistersGeneratorDependency()
        {
            Kernel.Register((kernel) => new EmptyConstructorClass()).As<EmptyConstructorClass>();

            Kernel.Factories.Should().ContainKey(typeof(EmptyConstructorClass));
            Kernel.Factories[typeof(EmptyConstructorClass)].Single().Should()
                .BeOfType<GeneratorFactory<EmptyConstructorClass>>();
        }

        [Test]
        public void RegistersSingletonGeneratorDependency()
        {
            Kernel.Register((kernel) => new EmptyConstructorClass()).AsSingleton<EmptyConstructorClass>();

            Kernel.Factories.Should().ContainKey(typeof(EmptyConstructorClass));
            Kernel.Factories[typeof(EmptyConstructorClass)].Single().Should()
                .BeOfType<SingletonGeneratorFactory<EmptyConstructorClass>>();
        }

        [Test]
        public void RegistersNamedDependency()
        {
            Kernel.Register<EmptyConstructorClass>().As<EmptyConstructorClass>("Empty");

            Kernel.Factories.Should().ContainKey(typeof(EmptyConstructorClass));
            var factory = Kernel.Factories[typeof(EmptyConstructorClass)].Single();

            factory.Should().BeOfType<Factory>();
            factory.Name.Should().Be("Empty");
        }

        [Test]
        public void RegistersNamedSingletonDependency()
        {
            Kernel.Register<EmptyConstructorClass>().AsSingleton<EmptyConstructorClass>("Empty");

            Kernel.Factories.Should().ContainKey(typeof(EmptyConstructorClass));
            var factory = Kernel.Factories[typeof(EmptyConstructorClass)].Single();

            factory.Should().BeOfType<SingletonFactory>();
            factory.Name.Should().Be("Empty");
        }

        [Test]
        public void RegistersNamedGeneratorDependency()
        {
            Kernel.Register((kernel) => new EmptyConstructorClass()).As<EmptyConstructorClass>("Empty");

            Kernel.Factories.Should().ContainKey(typeof(EmptyConstructorClass));
            var factory = Kernel.Factories[typeof(EmptyConstructorClass)].Single();

            factory.Should().BeOfType<GeneratorFactory<EmptyConstructorClass>>();
            factory.Name.Should().Be("Empty");
        }

        [Test]
        public void RegistersNamedSingletonGeneratorDependency()
        {
            Kernel.Register((kernel) => new EmptyConstructorClass()).AsSingleton<EmptyConstructorClass>("Empty");

            Kernel.Factories.Should().ContainKey(typeof(EmptyConstructorClass));
            var factory = Kernel.Factories[typeof(EmptyConstructorClass)].Single();

            factory.Should().BeOfType<SingletonGeneratorFactory<EmptyConstructorClass>>();
            factory.Name.Should().Be("Empty");
        }
    }
}