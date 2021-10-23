using CatApi.bindings;
using CatApi.types;
using CatApi.types.containers;
using CatDi.di;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class Objects
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
        }
    }
}