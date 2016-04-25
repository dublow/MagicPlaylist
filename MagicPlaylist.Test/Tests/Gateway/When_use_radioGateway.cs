﻿using MagicPlaylist.Test.Mocked.Gateway;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var tracks = radioGateway.GetRandomTracks();

            Assert.IsTrue(tracks.Any());
            Assert.AreEqual(10, tracks.Count());
        }
    }
}