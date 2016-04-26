using MagicPlaylist.Common.Loggers;
using MagicPlaylist.Common.Profilers;
using MagicPlaylist.Deezer;
using MagicPlaylist.Gateway;
using MagicPlaylist.Gateway.Models;
using MagicPlaylist.Web.Exceptions;
using Nancy;
using Nancy.ModelBinding;
using NLog;
using System;
using System.Linq;

namespace MagicPlaylist.Web.Modules
{
    public class HomeModule : NancyModule
    {
        private static Logger logger = LogManager.GetLogger("Module");
        public HomeModule(IRadioGateway radioGateway, IMagicPlaylistGateway magicPlaylistGateway, HttpDeezer httpDeezer)
        {
            Get["/"] = _ =>
            {
                using (var smartTimer = new SmartTimer((x, u) => ModuleLoggerInfo("Exit Home", x.Elapsed)))
                {
                    ModuleLoggerInfo("Exit Home");
                    return View["index.html"];
                }    
            };

            Get["/channel"] = _ =>
            {
                using (var smartTimer = new SmartTimer((x, u) => ModuleLoggerInfo("Exit Channel", x.Elapsed)))
                {
                    ModuleLoggerInfo("Enter Channel");
                    return View["channel.html"];
                }
            };

            Post["/playlist"] = parameters =>
            {
                try
                {
                    using (var smartTimer = new SmartTimer((x, u) => ModuleLoggerInfo("Exit Playlist", u, x.Elapsed)))
                    {
                        ModuleLoggerInfo("Enter Playlist");

                        // Manage user
                        var user = this.Bind<UserModel>();

                        if (user == null || user.Id == 0)
                            throw new MagicPlaylistException("UserId is null or empty");

                        smartTimer.SetUserId(user.Id);

                        if (string.IsNullOrEmpty(user.AccessToken))
                            throw new MagicPlaylistException(user.Id, "AccessToken is null or empty");

                        if (!magicPlaylistGateway.CanAddPlaylist(user.Id))
                            throw new MagicPlaylistException(user.Id, "User can't add playlist");

                        magicPlaylistGateway.AddOrUpdateUser(user);

                        // Get tracks
                        var tracks = radioGateway.GetRandomTracks(user.Id);
                        if (tracks == null || !tracks.Any())
                            throw new MagicPlaylistException(user.Id, "DbTracks is null or empty");

                        // Add playlist
                        var deezerPlaylist = httpDeezer.AddPlaylist(user.Id, user.AccessToken, "MagicPlaylist");
                        if (deezerPlaylist == null)
                            throw new MagicPlaylistException(user.Id, "DeezerPlaylist is null");
                        else if (deezerPlaylist.HasError)
                            throw new MagicPlaylistException(user.Id, string.Format("DeezerPlaylist error:[{0}][{1}][{2}]",
                                deezerPlaylist.Error.Type, deezerPlaylist.Error.Message, deezerPlaylist.Error.Code));

                        // Add tracks
                        var deezerTracks = httpDeezer.AddTracks(user.Id, user.AccessToken, deezerPlaylist.Id, tracks);
                        if (deezerTracks == null)
                            throw new MagicPlaylistException(user.Id, "DeezerTracks is null");
                        else if (deezerTracks.HasError)
                            throw new MagicPlaylistException(user.Id, string.Format("DeezerTracks error:[{0}][{1}][{2}]",
                                deezerTracks.Error.Type, deezerTracks.Error.Message, deezerTracks.Error.Code));


                        return Success(deezerPlaylist.PlaylistUrl);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
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

        private Response Success(string playlistUrl)
        {
            return Response.AsJson(new { success = true, playlistUrl = playlistUrl });
        }

        private void ModuleLoggerInfo(string message)
        {
            logger.Info(LoggerCreator.Info("Module", message));
        }

        private void ModuleLoggerInfo(string message, TimeSpan elasped)
        {
            logger.Info(LoggerCreator.Info("Module", message, elasped));
        }

        private void ModuleLoggerInfo(string message, int userId, TimeSpan elasped)
        {
            logger.Info(LoggerCreator.Info("Module", message, userId, elasped));
        }

        
    }
}
