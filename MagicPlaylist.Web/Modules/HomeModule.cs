using MagicPlaylist.Deezer;
using MagicPlaylist.Gateway;
using MagicPlaylist.Web.Models;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
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
                var provider = new SqlProvider(ConfigurationManager.ConnectionStrings["radio"].ConnectionString);
                var gateway = new RadioGateway(provider);
                var tracks = gateway.GetRandomTracks();
                var user = this.Bind<DeezerModel>();
                var deezer = HttpDeezer.Create(user.AccessToken, user.Id);
                var playlistId = deezer.AddPlaylist("MagicPlaylist");
                var created = deezer.AddTracks(playlistId, tracks);

                return Response.AsJson(new { success = true });
            };
        }
    }
}
