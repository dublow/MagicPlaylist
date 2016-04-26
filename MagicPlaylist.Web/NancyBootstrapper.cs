using Nancy;
using Nancy.TinyIoc;
using System.Configuration;
using MagicPlaylist.Gateway;
using MagicPlaylist.Deezer.Request;
using MagicPlaylist.Deezer.Builder;
using NLog.Config;

namespace MagicPlaylist.Web
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            var radioCnx = ConfigurationManager.ConnectionStrings["radio"].ConnectionString;
            var radioProvider = new SqlProvider(radioCnx);
            IRadioGateway radioGateway = new RadioGateway(radioProvider);

            var magicPlaylistCnx = ConfigurationManager.ConnectionStrings["magicPlaylist"].ConnectionString;
            var magicPlaylistProvider = new SqlProvider(magicPlaylistCnx);
            IMagicPlaylistGateway magicPlaylistGateway = new MagicPlaylistGateway(magicPlaylistProvider);

            container.Register(radioGateway);
            container.Register(magicPlaylistGateway);
            container.Register<IHttpWebRequest, HttpDeezerWebRequest>();
            container.Register<IHttpWebBuilder, HttpWebBuilder>();

           
        }
    }

    
}
