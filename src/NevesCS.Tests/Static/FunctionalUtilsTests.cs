using FluentAssertions;

using NevesCS.Static.Extensions;
using NevesCS.Static.Utils;
using NevesCS.Tests.Mocks;

namespace NevesCS.Tests.Static
{
    public class FunctionalUtilsTests
    {
        [Fact]
        public void Set_Should_Set()
        {
            const string str = "whatever";

            MockClass mockClass = new();
            mockClass.SetIfNotNull(m => m.UselessString = str);
            mockClass.UselessString.Should().Be(str);
        }

        [Fact]
        public void SetIfNotNull_Should_NotSet_IfNull()
        {
            const string str = "whatever";

            MockClass? mockClass = new();
            mockClass.SetIfNotNull(m => m.UselessString = str);
            mockClass.UselessString.Should().Be(str);

            MockClass? nullClass = null;
            nullClass.SetIfNotNull(m => m.UselessString = str);
            nullClass.Should().Be(null);
            nullClass!?.UselessString.Should().Be(null);
        }

        [Fact]
        public void Into_Should_Allow_ConvertPrimitiveIntoReference()
        {
            const string fakeName = "This is a name I need";
            MockClass mockClass = new() { UselessString = fakeName };

            FunctionalUtils.Into(mockClass, mock => new DataGame() { Name = mock.UselessString }).Name.Should().Be(fakeName);
            mockClass.Into(mock => new DataGame() { Name = mock.UselessString }).Name.Should().Be(fakeName);
        }

        [Fact]
        public void Into_Should_Allow_ConvertReferenceIntoTuple()
        {
            const string fakeName = "This is a name I need";
            const string fakeUselessName = "Whatever";
            MockClass mockClass = new() { UselessString = fakeName, UselessList = [fakeUselessName] };

            var (resultFakeName, resultUselessFakeName) = mockClass.Into(mock => (mock.UselessString, mock.UselessList.Single()));

            resultFakeName.Should().Be(fakeName);
            resultUselessFakeName.Should().Be(fakeUselessName);
        }
    }
}
