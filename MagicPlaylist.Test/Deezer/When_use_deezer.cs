using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicPlaylist.Test.Mocked;
using MagicPlaylist.Deezer.Models;
using MagicPlaylist.Deezer.Builder;
using MagicPlaylist.Deezer;
using MagicPlaylist.Test.Mocked.Deezer;
using MagicPlaylist.Test.Mocked.Deezer.Extensions;

namespace MagicPlaylist.Test.Deezer
{
    [TestFixture]
    public class When_use_deezer
    {
        [Test]
        public void When_add_playlist_success()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .SuccessPlaylistResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var deezer = new HttpDeezer(httpWebBuilder);

            var playlist = deezer.AddPlaylist("56789", "accessToken", "MagicPlaylist");
            Assert.AreEqual("12345", playlist.Id);
        }

        [Test]
        public void When_add_playlist_with_invalid_response()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .ErrorPlaylistResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var deezer = new HttpDeezer(httpWebBuilder);

            var playlist = deezer.AddPlaylist("56789", "accessToken", "MagicPlaylist");

            Assert.IsTrue(playlist.HasError);
            Assert.AreEqual("ParameterException", playlist.Error.Type);
            Assert.AreEqual("Wrong parameter: playlistid", playlist.Error.Message);
            Assert.AreEqual(500, playlist.Error.Code);
        }

        [Test]
        public void When_add_playlist_with_null_response()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .NullResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var deezer = new HttpDeezer(httpWebBuilder);

            var playlist = deezer.AddPlaylist("56789", "accessToken", "MagicPlaylist");
            Assert.IsNull(playlist);
        }

        [Test]
        public void When_add_tracks_success()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .SuccessTracksResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var deezer = new HttpDeezer(httpWebBuilder);

            var tracks = deezer.AddTracks("accessToken", "12345", new[] { "123", "456" });
            Assert.IsTrue(tracks.Success);
        }

        [Test]
        public void When_add_tracks_invalid_playlist_id()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .ErrorTracksInvalidPlaylistIdResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var deezer = new HttpDeezer(httpWebBuilder);

            var tracks = deezer.AddTracks("accessToken", "12345", new[] { "123", "456" });

            Assert.IsTrue(tracks.HasError);
            Assert.AreEqual("DataException", tracks.Error.Type);
            Assert.AreEqual("no data", tracks.Error.Message);
            Assert.AreEqual(800, tracks.Error.Code);
        }

        [Test]
        public void When_add_tracks_invalid_tracks_id()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .ErrorTracksInvalidTrackIdResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var deezer = new HttpDeezer(httpWebBuilder);

            var tracks = deezer.AddTracks("accessToken", "12345", new[] { "123", "456" });

            Assert.IsTrue(tracks.HasError);
            Assert.AreEqual("ParameterException", tracks.Error.Type);
            Assert.AreEqual("Wrong parameter: songs", tracks.Error.Message);
            Assert.AreEqual(500, tracks.Error.Code);
        }

        [Test]
        public void When_add_tracks_with_null_response()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .NullResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var deezer = new HttpDeezer(httpWebBuilder);

            var tracks = deezer.AddTracks("accessToken", "12345", new[] { "123", "456" });
            Assert.IsNull(tracks);
        }
    }
}
