using System.Collections.Generic;
using System.Linq;
using Cat.ast.api;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class BufferedEnumerableTest
    {
        [Test]
        public void CanPerformOneFullIteration()
        {
            var list = new List<int>{ 1,3,45,6,7};
            var buffer = new BufferedEnumerable<int>(list);

            buffer.ToList().Should().BeEquivalentTo(list);
        }
        [Test]
        public void CanPerformTwoFullIteration_WithReset()
        {
            var list = new List<int>{ 1,3,45,6,7};
            var buffer = new BufferedEnumerable<int>(list);

            buffer.ToList();
            buffer.ResetPointer();
            buffer.ToList().Should().BeEquivalentTo(list);
        }
        [Test]
        public void CanBeResetInTheMiddle()
        {
            var list = new List<int>{ 1,3,45,6,7};
            var buffer = new BufferedEnumerable<int>(list);

            buffer.Take(3).ToList();
            buffer.ResetPointer();
            buffer.ToList().Should().BeEquivalentTo(list);
        }
    }
}