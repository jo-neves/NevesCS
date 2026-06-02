using FluentAssertions;

using NevesCS.Static.Constants.Values;
using NevesCS.Static.Extensions;
using NevesCS.Static.Utils;
using NevesCS.Tests.Mocks;

namespace NevesCS.Tests.Static
{
    public class BinaryUtilsTests
    {
        [Fact]
        public void Should_SerializeAndDeserialize_AnInstanceOfAnObject()
        {
            var input = new DataGame()
            {
                AppId = Ints.One,
                Owners = "CD Projekt",
                Developer = "CD Projekt Red",
                Name = "The Witcher 3: Wild Hunt",
                Genre = "Action role-playing",
                Ccu = 10571,
                //ReleaseDate = new DateTime(2015, 05, 19, 00, 00, 00, DateTimeKind.Unspecified),
            };

            var serializedResult1 = BinaryUtils.SerializeObject(input);
            var serializedResult2 = input.SerializeObjectIntoRawBytes();

            var deserializedResult1 = BinaryUtils.DeserializeByteObject<DataGame>(serializedResult1);
            var deserializedResult2 = serializedResult2?.DeserializeRawByteObject<DataGame>();

            deserializedResult1.Should().BeEquivalentTo(input);
            deserializedResult2.Should().BeEquivalentTo(input);

            var reserializedResult1 = BinaryUtils.SerializeObject(deserializedResult1);
            var reserializedResult2 = deserializedResult2.SerializeObjectIntoRawBytes();

            reserializedResult1.Should().BeEquivalentTo(serializedResult1);
            reserializedResult2.Should().BeEquivalentTo(serializedResult2);
        }
    }
}
