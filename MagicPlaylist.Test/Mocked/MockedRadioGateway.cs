﻿using MagicPlaylist.Gateway;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Test.Mocked
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
                .Setup(x => x.GetRandomTracks())
                .Returns(tracks);

            return this;
        }

        public IRadioGateway Build()
        {
            return _mockRadioGateway.Object;
        }
    }
}
