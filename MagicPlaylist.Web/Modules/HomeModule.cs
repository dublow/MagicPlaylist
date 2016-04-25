using MagicPlaylist.Deezer;
using MagicPlaylist.Gateway;
using MagicPlaylist.Gateway.Models;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Linq;

namespace MagicPlaylist.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IRadioGateway radioGateway, IMagicPlaylistGateway magicPlaylistGateway, HttpDeezer httpDeezer)
        {
            Get["/"] = _ =>
            {
                return View["index.html"];
            };

            Get["/channel"] = _ =>
            {
                return View["channel.html"];
            };

            Post["/playlist"] = parameters =>
            {
                try
                {
                    var user = this.Bind<UserModel>();

                    if (user == null || user.Id == 0)
                        throw new ArgumentNullException("user.id", "null value");

                    magicPlaylistGateway.AddOrUpdateUser(user);

                    var tracks = radioGateway.GetRandomTracks();
                    if (tracks == null || !tracks.Any())
                        return Fail();

                    var deezerPlaylist = httpDeezer.AddPlaylist(user.Id.ToString(), user.AccessToken, "MagicPlaylist");
                    if (deezerPlaylist == null && deezerPlaylist.HasError)
                        return Fail();

                    var deezerTracks = httpDeezer.AddTracks(user.AccessToken, deezerPlaylist.Id, tracks);
                    if (deezerTracks == null && deezerTracks.HasError)
                        return Fail();

                    return Success();
                }
                catch (Exception ex)
                {
                    var error = new ErrorModel(ex.GetType().Name, ex.Message, ex.StackTrace);
                    magicPlaylistGateway.AddError(error);
                    return Fail();
                }
            };
        }

        private Response Fail()
        {
            return Response.AsJson(new { success = false }, HttpStatusCode.InternalServerError);
        }

        private Response Success()
        {
            return Response.AsJson(new { success = true });
        }
    }
}
