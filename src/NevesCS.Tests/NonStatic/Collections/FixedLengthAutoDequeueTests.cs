using FluentAssertions;

using NevesCS.NonStatic.Collections;

namespace NevesCS.Tests.NonStatic.Collections
{
    public class FixedLengthAutoDequeueTests
    {
        [Fact]
        public void FixedLengthAutoDequeue_KeepsTheSameLength()
        {
            const int fixedLen = 21;
            const int lenAtStart = 3;
            var queue = new FixedSizeAutoDequeue<int>(fixedLen, [1, 2, 3]);

            queue.Count().Should().Be(lenAtStart);
            queue.Should().BeEquivalentTo([1, 2, 3]);

            for (int i = 0; i < 3; ++i)
            {
                queue.Enqueue(lenAtStart + 1 + i);
            }

            const int lenBeforeOverflow = (lenAtStart + 3);

            queue.Count().Should().Be(lenBeforeOverflow);
            queue.Should().BeEquivalentTo([1, 2, 3, 4, 5, 6]);

            for (int i = 0; i < fixedLen; ++i)
            {
                queue.Enqueue(lenBeforeOverflow + 1 + i);
            }

            queue.Count().Should().Be(fixedLen);
            queue.Should().BeEquivalentTo([7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27]);
        }
    }
}
