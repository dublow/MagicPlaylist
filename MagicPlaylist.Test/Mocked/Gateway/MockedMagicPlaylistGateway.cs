using MagicPlaylist.Gateway;
using MagicPlaylist.Gateway.Models;
using Moq;
using System.Collections.Generic;

namespace MagicPlaylist.Test.Mocked.Gateway
{
    public class MockedMagicPlaylistGateway
    {
        private readonly Mock<IMagicPlaylistGateway> _mockMagicPlaylistGateway;
        public readonly Dictionary<int, UserModel> _userTable;
        public readonly List<ErrorModel> _logTable;

        private MockedMagicPlaylistGateway()
        {
            _mockMagicPlaylistGateway = new Mock<IMagicPlaylistGateway>();
            _userTable = new Dictionary<int, UserModel>();
            _logTable = new List<ErrorModel>();

            // AddOrUpdateUser
            _mockMagicPlaylistGateway
                .Setup(x => x.AddOrUpdateUser(It.IsAny<UserModel>()))
                .Callback<UserModel>(userModelResult => {
                    if (_userTable.ContainsKey(userModelResult.Id))
                        _userTable[userModelResult.Id] = userModelResult;
                    else
                        _userTable.Add(userModelResult.Id, userModelResult);
                });

            // AddError
            _mockMagicPlaylistGateway
                .Setup(x => x.AddError(It.IsAny<ErrorModel>()))
                .Callback<ErrorModel>(errorModelresult => {
                    _logTable.Add(errorModelresult);
                });
        }

        public static MockedMagicPlaylistGateway Create()
        {
            return new MockedMagicPlaylistGateway();
        }

        public MockedMagicPlaylistGateway CanAddPlaylist(bool value)
        {
            _mockMagicPlaylistGateway
                .Setup(x => x.CanAddPlaylist(It.IsAny<int>()))
                .Returns(value);
            return this;
        }
        public MockedMagicPlaylistGateway AddUser(UserModel userModel)
        {
            _userTable.Add(userModel.Id, userModel);
            return this;
        }

        public IMagicPlaylistGateway Build()
        {
            return _mockMagicPlaylistGateway.Object;
        }
    }
}
