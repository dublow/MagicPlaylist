using MagicPlaylist.Gateway;
using Moq;
using System.Collections.Generic;

namespace MagicPlaylist.Test.Mocked.Gateway
{
    public class MockedRadioGateway
    {
        private readonly Mock<IRadioGateway> _mockRadioGateway;

        private MockedRadioGateway()
        {
            _mockRadioGateway = new Mock<IRadioGateway>();
        }

        public static MockedRadioGateway Create()
        {
            return new MockedRadioGateway();
        }

        public MockedRadioGateway SetTracks(IEnumerable<string> tracks)
        {
            _mockRadioGateway
                .Setup(x => x.GetRandomTracks(1))
                .Returns(tracks);

            return this;
        }

        public IRadioGateway Build()
        {
            return _mockRadioGateway.Object;
        }
    }
}
