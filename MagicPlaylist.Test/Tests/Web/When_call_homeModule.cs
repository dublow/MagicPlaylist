using MagicPlaylist.Deezer;
using MagicPlaylist.Deezer.Builder;
using MagicPlaylist.Gateway;
using MagicPlaylist.Test.Mocked.Deezer;
using MagicPlaylist.Test.Mocked.Deezer.Extensions;
using MagicPlaylist.Test.Mocked.Gateway;
using MagicPlaylist.Test.Mocked.Gateway.Extensions;
using MagicPlaylist.Web.Modules;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Test.Tests.Web
{
    [TestFixture]
    public class When_call_homeModule
    {
        [Test]
        public void When_get_index_must_be_success()
        {
            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(BuildSuccessMagicPlaylistGateway());
                cfg.Dependency(BuildSuccessHttpDeezer());
            });

            var response = browser.Get("/");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("text/html", response.ContentType);
        }

        [Test]
        public void When_get_channel_must_be_success()
        {
            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(BuildSuccessMagicPlaylistGateway());
                cfg.Dependency(BuildSuccessHttpDeezer());
            });

            var response = browser.Get("/channel");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("text/html", response.ContentType);
        }

        [Test]
        public void When_post_playlist_must_be_success()
        {
            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(BuildSuccessMagicPlaylistGateway());
                cfg.Dependency(BuildSuccessHttpDeezer());
            });

            var response = browser.Post("/playlist", (with) => {
                with.HttpRequest();
                with.FormValue("id", "1");
                with.FormValue("accessToken", "abcde");
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("{\"success\":true}", response.Body.AsString());
        }

        private HttpDeezer BuildSuccessHttpDeezer()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .SuccessPlaylistResponse()
                                    .SuccessTracksResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);

            return new HttpDeezer(httpWebBuilder);
        }

        private IMagicPlaylistGateway BuildSuccessMagicPlaylistGateway()
        {
            return MockedMagicPlaylistGateway
                    .Create()
                    .Build();
        }

        private IRadioGateway BuildSuccessRadioGateway()
        {
            return MockedRadioGateway
                    .Create()
                    .TracksGenerator(10)
                    .Build();
        }
    }
}
