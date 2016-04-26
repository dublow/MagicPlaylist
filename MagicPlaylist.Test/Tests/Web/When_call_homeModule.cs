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
using System.Linq;

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
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway.Create();

            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(mockedMagicPlaylistGateway.Build());
                cfg.Dependency(BuildSuccessHttpDeezer());
            });

            var response = browser.Post("/playlist", (with) => {
                with.HttpRequest();
                with.FormValue("id", "1");
                with.FormValue("accessToken", "abcde");
                with.FormValue("firstname", "Nicolas");
                with.FormValue("lastname", "Delfour");
                with.FormValue("email", "nicolas.delfour@test.com");
                with.FormValue("gender", "M");
                with.FormValue("name", "Nico");
                with.FormValue("country", "FR");
                with.FormValue("lang", "FR");
                with.FormValue("birthday", "1980-02-25");
            });

            var userTable = mockedMagicPlaylistGateway._userTable;
            var userStored = userTable
                                .Where(x => x.Key == 1)
                                .Select(x => x.Value)
                                .SingleOrDefault();

            Assert.AreEqual(1, userTable.Count);
            Assert.IsNotNull(userStored);
            Assert.AreEqual(1, userStored.Id);
            Assert.AreEqual("abcde", userStored.AccessToken);
            Assert.AreEqual("Nicolas", userStored.Firstname);
            Assert.AreEqual("Delfour", userStored.Lastname);
            Assert.AreEqual("nicolas.delfour@test.com", userStored.Email);
            Assert.AreEqual("M", userStored.Gender);
            Assert.AreEqual("Nico", userStored.Name);
            Assert.AreEqual("FR", userStored.Country);
            Assert.AreEqual("FR", userStored.Lang);
            Assert.AreEqual("1980-02-25", userStored.Birthday);

            Assert.AreEqual(0, mockedMagicPlaylistGateway._logTable.Count);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("{\"success\":true,\"playlistUrl\":\"https://www.deezer.com/playlist/12345\"}", response.Body.AsString());
        }

        [Test]
        public void When_post_playlist_with_known_user_must_be_success()
        {
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway.Create();
            mockedMagicPlaylistGateway.AddFastUser(1, "OtherNicolas", "OtherDelfour", "othernicolas.delfour@test.com");

            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(mockedMagicPlaylistGateway.Build());
                cfg.Dependency(BuildSuccessHttpDeezer());
            });

            var response = browser.Post("/playlist", (with) => {
                with.HttpRequest();
                with.FormValue("id", "1");
                with.FormValue("accessToken", "abcde");
                with.FormValue("firstname", "OtherNicolas");
                with.FormValue("lastname", "OtherDelfour");
                with.FormValue("email", "othernicolas.delfour@test.com");
                with.FormValue("gender", "M");
                with.FormValue("name", "Nico");
                with.FormValue("country", "FR");
                with.FormValue("lang", "FR");
                with.FormValue("birthday", "1980-02-25");
            });

            var userTable = mockedMagicPlaylistGateway._userTable;
            var userStored = userTable
                                .Where(x => x.Key == 1)
                                .Select(x => x.Value)
                                .SingleOrDefault();

            Assert.AreEqual(1, userTable.Count);
            Assert.IsNotNull(userStored);
            Assert.AreEqual(1, userStored.Id);
            Assert.AreEqual("abcde", userStored.AccessToken);
            Assert.AreEqual("OtherNicolas", userStored.Firstname);
            Assert.AreEqual("OtherDelfour", userStored.Lastname);
            Assert.AreEqual("othernicolas.delfour@test.com", userStored.Email);
            Assert.AreEqual("M", userStored.Gender);
            Assert.AreEqual("Nico", userStored.Name);
            Assert.AreEqual("FR", userStored.Country);
            Assert.AreEqual("FR", userStored.Lang);
            Assert.AreEqual("1980-02-25", userStored.Birthday);

            Assert.AreEqual(0, mockedMagicPlaylistGateway._logTable.Count);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("{\"success\":true,\"playlistUrl\":\"https://www.deezer.com/playlist/12345\"}", response.Body.AsString());
        }

        [Test]
        public void When_post_playlist_with_no_user_id_must_be_failed()
        {
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway.Create();

            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(mockedMagicPlaylistGateway.Build());
                cfg.Dependency(BuildSuccessHttpDeezer());
            });

            var response = browser.Post("/playlist", (with) => {
                with.HttpRequest();
                with.FormValue("accessToken", "abcde");
                with.FormValue("firstname", "Nicolas");
                with.FormValue("lastname", "Delfour");
                with.FormValue("email", "nicolas.delfour@test.com");
                with.FormValue("gender", "M");
                with.FormValue("name", "Nico");
                with.FormValue("country", "FR");
                with.FormValue("lang", "FR");
                with.FormValue("birthday", "1980-02-25");
            });

            var logTable = mockedMagicPlaylistGateway._logTable;
            var error = logTable.SingleOrDefault();

            Assert.AreEqual(1, logTable.Count);
            Assert.IsNotNull(error);
            Assert.AreEqual("MagicPlaylistException", error.errorType);
            Assert.AreEqual("UserId is null or empty", error.message);
            Assert.IsNotEmpty(error.stackTrace);
            Assert.AreEqual(0, mockedMagicPlaylistGateway._userTable.Count);
            
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("{\"success\":false}", response.Body.AsString());
        }

        [Test]
        public void When_post_playlist_with_no_access_token_must_be_failed()
        {
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway.Create();

            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(mockedMagicPlaylistGateway.Build());
                cfg.Dependency(BuildSuccessHttpDeezer());
            });

            var response = browser.Post("/playlist", (with) => {
                with.HttpRequest();
                with.FormValue("id", "1");
                with.FormValue("firstname", "Nicolas");
                with.FormValue("lastname", "Delfour");
                with.FormValue("email", "nicolas.delfour@test.com");
                with.FormValue("gender", "M");
                with.FormValue("name", "Nico");
                with.FormValue("country", "FR");
                with.FormValue("lang", "FR");
                with.FormValue("birthday", "1980-02-25");
            });

            var logTable = mockedMagicPlaylistGateway._logTable;
            var error = logTable.SingleOrDefault();

            Assert.AreEqual(1, logTable.Count);
            Assert.IsNotNull(error);
            Assert.AreEqual("MagicPlaylistException", error.errorType);
            Assert.AreEqual("AccessToken is null or empty", error.message);
            Assert.IsNotEmpty(error.stackTrace);
            Assert.AreEqual(0, mockedMagicPlaylistGateway._userTable.Count);

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("{\"success\":false}", response.Body.AsString());
        }

        [Test]
        public void When_post_playlist_with_no_tracks_must_be_failed()
        {
            var radioGateway = MockedRadioGateway
                    .Create()
                    .TracksGenerator(0)
                    .Build();

            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway.Create();

            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(radioGateway);
                cfg.Dependency(mockedMagicPlaylistGateway.Build());
                cfg.Dependency(BuildSuccessHttpDeezer());
            });

            var response = browser.Post("/playlist", (with) => {
                with.HttpRequest();
                with.FormValue("id", "1");
                with.FormValue("accessToken", "abcde");
                with.FormValue("firstname", "Nicolas");
                with.FormValue("lastname", "Delfour");
                with.FormValue("email", "nicolas.delfour@test.com");
                with.FormValue("gender", "M");
                with.FormValue("name", "Nico");
                with.FormValue("country", "FR");
                with.FormValue("lang", "FR");
                with.FormValue("birthday", "1980-02-25");
            });

            var logTable = mockedMagicPlaylistGateway._logTable;
            var error = logTable.SingleOrDefault();

            Assert.AreEqual(1, logTable.Count);
            Assert.IsNotNull(error);
            Assert.AreEqual("MagicPlaylistException", error.errorType);
            Assert.AreEqual("Tracks is null or empty", error.message);
            Assert.IsNotEmpty(error.stackTrace);
            Assert.AreEqual(1, mockedMagicPlaylistGateway._userTable.Count);

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("{\"success\":false}", response.Body.AsString());
        }

        [Test]
        public void When_post_playlist_with_null_playlist_must_be_failed()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .NullResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var httpDeezer = new HttpDeezer(httpWebBuilder);
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway.Create();

            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(mockedMagicPlaylistGateway.Build());
                cfg.Dependency(httpDeezer);
            });

            var response = browser.Post("/playlist", (with) => {
                with.HttpRequest();
                with.FormValue("id", "1");
                with.FormValue("accessToken", "abcde");
                with.FormValue("firstname", "Nicolas");
                with.FormValue("lastname", "Delfour");
                with.FormValue("email", "nicolas.delfour@test.com");
                with.FormValue("gender", "M");
                with.FormValue("name", "Nico");
                with.FormValue("country", "FR");
                with.FormValue("lang", "FR");
                with.FormValue("birthday", "1980-02-25");
            });

            var logTable = mockedMagicPlaylistGateway._logTable;
            var error = logTable.SingleOrDefault();

            Assert.AreEqual(1, logTable.Count);
            Assert.IsNotNull(error);
            Assert.AreEqual("MagicPlaylistException", error.errorType);
            Assert.AreEqual("DeezerPlaylist is null", error.message);
            Assert.IsNotEmpty(error.stackTrace);
            Assert.AreEqual(1, mockedMagicPlaylistGateway._userTable.Count);

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("{\"success\":false}", response.Body.AsString());
        }

        [Test]
        public void When_post_playlist_with_error_playlist_must_be_failed()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .ErrorPlaylistResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var httpDeezer = new HttpDeezer(httpWebBuilder);
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway.Create();

            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(mockedMagicPlaylistGateway.Build());
                cfg.Dependency(httpDeezer);
            });

            var response = browser.Post("/playlist", (with) => {
                with.HttpRequest();
                with.FormValue("id", "1");
                with.FormValue("accessToken", "abcde");
                with.FormValue("firstname", "Nicolas");
                with.FormValue("lastname", "Delfour");
                with.FormValue("email", "nicolas.delfour@test.com");
                with.FormValue("gender", "M");
                with.FormValue("name", "Nico");
                with.FormValue("country", "FR");
                with.FormValue("lang", "FR");
                with.FormValue("birthday", "1980-02-25");
            });

            var logTable = mockedMagicPlaylistGateway._logTable;
            var error = logTable.SingleOrDefault();

            Assert.AreEqual(1, logTable.Count);
            Assert.IsNotNull(error);
            Assert.AreEqual("MagicPlaylistException", error.errorType);
            Assert.AreEqual("DeezerPlaylist error:[ParameterException][Wrong parameter: playlistid][500]", error.message);
            Assert.IsNotEmpty(error.stackTrace);
            Assert.AreEqual(1, mockedMagicPlaylistGateway._userTable.Count);

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("{\"success\":false}", response.Body.AsString());
        }

        [Test]
        public void When_post_playlist_with_null_tracks_must_be_failed()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .SuccessPlaylistResponse()
                                    .NullResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var httpDeezer = new HttpDeezer(httpWebBuilder);
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway.Create();

            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(mockedMagicPlaylistGateway.Build());
                cfg.Dependency(httpDeezer);
            });

            var response = browser.Post("/playlist", (with) => {
                with.HttpRequest();
                with.FormValue("id", "1");
                with.FormValue("accessToken", "abcde");
                with.FormValue("firstname", "Nicolas");
                with.FormValue("lastname", "Delfour");
                with.FormValue("email", "nicolas.delfour@test.com");
                with.FormValue("gender", "M");
                with.FormValue("name", "Nico");
                with.FormValue("country", "FR");
                with.FormValue("lang", "FR");
                with.FormValue("birthday", "1980-02-25");
            });

            var logTable = mockedMagicPlaylistGateway._logTable;
            var error = logTable.SingleOrDefault();

            Assert.AreEqual(1, logTable.Count);
            Assert.IsNotNull(error);
            Assert.AreEqual("MagicPlaylistException", error.errorType);
            Assert.AreEqual("DeezerTracks is null", error.message);
            Assert.IsNotEmpty(error.stackTrace);
            Assert.AreEqual(1, mockedMagicPlaylistGateway._userTable.Count);

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("{\"success\":false}", response.Body.AsString());
        }

        [Test]
        public void When_post_playlist_with_error_tracks_must_be_failed()
        {
            var httpWebRequest = MockedHttpWebRequest
                                    .Create()
                                    .SuccessPlaylistResponse()
                                    .ErrorTracksInvalidTrackIdResponse()
                                    .Build();

            var httpWebBuilder = new HttpWebBuilder(httpWebRequest);
            var httpDeezer = new HttpDeezer(httpWebBuilder);
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway.Create();

            var browser = new Browser(cfg =>
            {
                cfg.Module<HomeModule>();
                cfg.Dependency(BuildSuccessRadioGateway());
                cfg.Dependency(mockedMagicPlaylistGateway.Build());
                cfg.Dependency(httpDeezer);
            });

            var response = browser.Post("/playlist", (with) => {
                with.HttpRequest();
                with.FormValue("id", "1");
                with.FormValue("accessToken", "abcde");
                with.FormValue("firstname", "Nicolas");
                with.FormValue("lastname", "Delfour");
                with.FormValue("email", "nicolas.delfour@test.com");
                with.FormValue("gender", "M");
                with.FormValue("name", "Nico");
                with.FormValue("country", "FR");
                with.FormValue("lang", "FR");
                with.FormValue("birthday", "1980-02-25");
            });

            var logTable = mockedMagicPlaylistGateway._logTable;
            var error = logTable.SingleOrDefault();

            Assert.AreEqual(1, logTable.Count);
            Assert.IsNotNull(error);
            Assert.AreEqual("MagicPlaylistException", error.errorType);
            Assert.AreEqual("DeezerTracks error:[ParameterException][Wrong parameter: songs][500]", error.message);
            Assert.IsNotEmpty(error.stackTrace);
            Assert.AreEqual(1, mockedMagicPlaylistGateway._userTable.Count);

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.ContentType);
            Assert.AreEqual("{\"success\":false}", response.Body.AsString());
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
