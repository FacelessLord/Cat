using System;
using System.Collections.Generic;
using CatDi.di;
using CatDi.di.exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace CatDi_Tests
{
    [TestFixture]
    public class ResolutionTests
    {
        public Kernel Kernel;

        [SetUp]
        public void Setup()
        {
            Kernel = new Kernel();
        }

        [Test]
        public void ResolveClass_WithoutDependencies()
        {
            Kernel.Register<EmptyConstructorClass>().As<EmptyConstructorClass>();

            Kernel.Resolve<EmptyConstructorClass>().Should().BeOfType<EmptyConstructorClass>();
        }

        [Test]
        public void ResolveClass_WithOneDependency()
        {
            Kernel.Register<EmptyConstructorClass>().As<EmptyConstructorClass>();
            Kernel.Register<OneDependencyClass>().As<OneDependencyClass>();

            var result = Kernel.Resolve<OneDependencyClass>();
            result.Should().BeOfType<OneDependencyClass>();
            result.EmptyConstructorClass.Should().BeOfType<EmptyConstructorClass>();
        }

        [Test]
        public void ResolveClass_WithManyDependencies()
        {
            Kernel.Register<EmptyConstructorClass>().As<EmptyConstructorClass>();
            Kernel.Register<OneDependencyClass>().As<OneDependencyClass>();
            Kernel.Register<ManyDependencyClass>().As<ManyDependencyClass>();

            var result = Kernel.Resolve<ManyDependencyClass>();
            result.Should().BeOfType<ManyDependencyClass>();
            result.EmptyConstructorClass.Should().BeOfType<EmptyConstructorClass>();
            result.EmptyConstructorClass2.Should().BeOfType<EmptyConstructorClass>();
            result.OneDependencyClass.Should().BeOfType<OneDependencyClass>();
            result.OneDependencyClass2.Should().BeOfType<OneDependencyClass>();
        }

        [Test]
        public void ResolveClass_WithInterfaceDependencies()
        {
            Kernel.Register<TestClass>().As<ITestClass>();
            Kernel.Register<TestDependentClass>().As<TestDependentClass>();

            var result = Kernel.Resolve<TestDependentClass>();
            result.Should().BeOfType<TestDependentClass>();
            result.TestClass.Should().BeOfType<TestClass>();
        }

        [Test]
        public void ResolveSameObject_WhenSingletonFactory()
        {
            Kernel.Register<TestClass>().AsSingleton<ITestClass>();

            var A = Kernel.Resolve<ITestClass>();
            var B = Kernel.Resolve<ITestClass>();

            A.Should().BeSameAs(B);
        }


        [Test]
        public void Throw_WhenDependency_NotRegistered()
        {
            Kernel.Register<OneDependencyClass>().As<OneDependencyClass>();
            Kernel.Register<ManyDependencyClass>().As<ManyDependencyClass>();

            Func<ManyDependencyClass> result = () => Kernel.Resolve<ManyDependencyClass>();

            result.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        public void Throw_WhenDependency_IsCyclic_And_Automatic()
        {
            Kernel.Register<CyclicDependencyClassA>().As<CyclicDependencyClassA>();
            Kernel.Register<CyclicDependencyClassB>().As<CyclicDependencyClassB>();

            Func<CyclicDependencyClassB> result = () => Kernel.Resolve<CyclicDependencyClassB>();

            result.Should().Throw<CatDiCyclicImportException>();
        }

        [Test]
        public void Throw_WhenDependency_IsCyclic_And_HalfManual()
        {
            Kernel.Register(resolver => new CyclicDependencyClassA(resolver.Resolve<CyclicDependencyClassB>()))
                .As<CyclicDependencyClassA>();
            Kernel.Register<CyclicDependencyClassB>().As<CyclicDependencyClassB>();

            Func<CyclicDependencyClassB> result = () => Kernel.Resolve<CyclicDependencyClassB>();

            result.Should().Throw<CatDiCyclicImportException>();
        }

        [Test]
        public void Throw_WhenDependency_IsCyclic_And_Manual()
        {
            Kernel.Register(resolver => new CyclicDependencyClassA(resolver.Resolve<CyclicDependencyClassB>()))
                .As<CyclicDependencyClassA>();
            Kernel.Register(resolver => new CyclicDependencyClassB(resolver.Resolve<CyclicDependencyClassA>()))
                .As<CyclicDependencyClassB>();

            Func<CyclicDependencyClassB> result = () => Kernel.Resolve<CyclicDependencyClassB>();

            result.Should().Throw<CatDiCyclicImportException>();
        }
    }
}