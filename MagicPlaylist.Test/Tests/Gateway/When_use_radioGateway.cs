using MagicPlaylist.Test.Mocked.Gateway;
using NUnit.Framework;
using System.Linq;
using MagicPlaylist.Test.Mocked.Gateway.Extensions;

namespace MagicPlaylist.Test.Tests.Gateway
{
    [TestFixture]
    public class When_use_radioGateway
    {
        [Test]
        public void When_load_fake_tracks()
        {
            var radioGateway = MockedRadioGateway
                .Create()
                .TracksGenerator(10)
                .Build();

            var tracks = radioGateway.GetRandomTracks(1);

            Assert.IsTrue(tracks.Any());
            Assert.AreEqual(10, tracks.Count());
        }
    }
}
