using MagicPlaylist.Gateway.Models;
using MagicPlaylist.Test.Mocked.Gateway;
using MagicPlaylist.Test.Mocked.Gateway.Extensions;
using NUnit.Framework;

namespace MagicPlaylist.Test.Tests.Gateway
{
    [TestFixture]
    public class When_use_magicPlaylistGateway
    {
        [Test]
        public void When_add_fake_user()
        {
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway
                .Create()
                .CanAddPlaylist(true)
                .AddFastUser(1, "Nicolas", "Delfour", "nicolas.delfour@test.com");

            var magicPlaylistGateway = mockedMagicPlaylistGateway.Build();

            var user = new UserModel {
                Id = 2,
                Firstname = "Toto",
                Lastname = "Tata",
                Email = "toto.tata@test.com",
                Birthday = "1990-07-22",
                Country = "US",
                Gender = "F",
                Lang = "US",
                Name = "Toto - Tata"
            };

            magicPlaylistGateway.AddOrUpdateUser(user);

            Assert.AreEqual(2, mockedMagicPlaylistGateway._userTable.Count);
        }

        [Test]
        public void When_update_fake_user()
        {
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway
                .Create()
                .CanAddPlaylist(true)
                .AddFastUser(1, "Nicolas", "Delfour", "nicolas.delfour@test.com");

            var magicPlaylistGateway = mockedMagicPlaylistGateway.Build();

            var user = new UserModel
            {
                Id = 1,
                Firstname = "Toto",
                Lastname = "Tata",
                Email = "toto.tata@test.com",
                Birthday = "1990-07-22",
                Country = "US",
                Gender = "F",
                Lang = "US",
                Name = "Toto - Tata"
            };

            magicPlaylistGateway.AddOrUpdateUser(user);

            Assert.AreEqual(1, mockedMagicPlaylistGateway._userTable.Count);
        }

        [Test]
        public void When_add_error()
        {
            var mockedMagicPlaylistGateway = MockedMagicPlaylistGateway
                                                .Create()
                                                .CanAddPlaylist(true);

            var magicPlaylistGateway = mockedMagicPlaylistGateway.Build();

            var error = new ErrorModel("Exception", "Invalid fake data", string.Empty);

            magicPlaylistGateway.AddError(error);

            Assert.AreEqual(1, mockedMagicPlaylistGateway._logTable.Count);
        }
    }
}
