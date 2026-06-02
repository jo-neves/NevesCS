using FluentAssertions;

using NevesCS.Static.Extensions;
using NevesCS.Static.Utils;
using NevesCS.Tests.Mocks;

namespace NevesCS.Tests.Static
{
    public class ObjectUtilsTests
    {
        [Fact]
        public void ThrowIfNull_Should_Throw_IfNullClass()
        {
            MockClass mockClass = null!;

            MockClass action1() => mockClass.ThrowIfNull(nameof(mockClass));
            MockClass action2() => ObjectUtils.ThrowIfNull(mockClass, nameof(mockClass));

            Array.ForEach(new[] { action1, action2 }, action => action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(mockClass)));
        }

        [Fact]
        public void ThrowIfNull_Should_NotThrow_IfNotNullClass()
        {
            MockClass mockClass = new();

            MockClass action1() => mockClass.ThrowIfNull(nameof(mockClass));
            MockClass action2() => ObjectUtils.ThrowIfNull(mockClass, nameof(mockClass));

            Array.ForEach(new[] { action1, action2 }, action => action.Should().NotThrow());
        }

        [Fact]
        public void Enumerate_Should_EnumerateAllParams()
        {
            var result = ObjectUtils.Enumerate(new DataGame() { AppId = 1 }, new DataGame() { AppId = 2 }, new DataGame() { AppId = 3 }).ToArray();

            result[0].AppId.Should().Be(1);
            result[1].AppId.Should().Be(2);
            result[2].AppId.Should().Be(3);
        }

        [Fact]
        public void Enumerate_Should_EnumerateXTimes()
        {
            var subject = new DataGame() { AppId = 123 };

            subject.Enumerate(0).ToArray().Length.Should().Be(0);
            subject.Enumerate(1).ToArray().Length.Should().Be(1);
            subject.Enumerate(3).ToArray().Length.Should().Be(3);
            subject.Enumerate(10).ToArray().Length.Should().Be(10);
        }

        [Fact]
        public void EnumerateClones_Should_EnumerateClonesXTimes()
        {
            DataGame subject = new() { AppId = 123 };

            subject.EnumerateClones<DataGame>(0).ToArray().Length.Should().Be(0);
            subject.EnumerateClones<DataGame>(1).ToArray().Length.Should().Be(1);
            subject.EnumerateClones<DataGame>(3).ToArray().Length.Should().Be(3);
            subject.EnumerateClones<DataGame>(10).ToArray().Length.Should().Be(10);
        }

        [Fact]
        public void ToArray_Should_CreateNewArrayWithTheObject()
        {
            DataGame subject = new() { AppId = 123 };

            var array1 = subject.ToArray();
            var array2 = ObjectUtils.ToArray(subject);

            array1[0].Should().BeSameAs(subject);
            array1.Should().HaveCount(1);
            array2[0].Should().BeSameAs(subject);
            array2.Should().HaveCount(1);
        }
    }
}
