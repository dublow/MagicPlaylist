using MagicPlaylist.Gateway.Models;

namespace MagicPlaylist.Test.Mocked.Gateway.Extensions
{
    public static class MagicPlaylistExtension
    {
        public static MockedMagicPlaylistGateway AddFastUser(this MockedMagicPlaylistGateway mockedMagicPlaylistGateway, 
            int id, string firstname, string lastname, string email)
        {
            var userModel = new UserModel
            {
                Id = id,
                Firstname = firstname,
                Lastname = lastname,
                Email = email,
                Birthday = "1980-02-25",
                Country = "FR",
                Gender = "M",
                Lang = "FR",
                Name = string.Format("{0} - {1}", firstname, lastname)
            };

            return mockedMagicPlaylistGateway.AddUser(userModel);
        }
    }
}
