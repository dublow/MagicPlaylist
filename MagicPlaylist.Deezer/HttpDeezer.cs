using MagicPlaylist.Deezer.Builder;
using MagicPlaylist.Deezer.Models;
using NLog;
using System.Collections.Generic;

namespace MagicPlaylist.Deezer
{
    public class HttpDeezer
    {
        private static Logger logger = LogManager.GetLogger("Deezer");

        private const string baseUri = "http://api.deezer.com/";
        private readonly HttpWebBuilder _httpWebBuilder;
        public HttpDeezer(HttpWebBuilder httpWebBuilder)
        {
            _httpWebBuilder = httpWebBuilder;
        }

        public DeezerPlaylist AddPlaylist(int userId, string accessToken, string title)
        {
            
            try
            {
                logger.Info("[userId:{0}]AddPlaylist", userId);
                var result = _httpWebBuilder
                            .Post(GetPlaylistUri(userId, accessToken, title), userId)
                            .GetReponseToJson<DeezerPlaylist>();

                return result;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            
        }

        public DeezerTrack AddTracks(int userId, string accessToken, string playlistId, IEnumerable<string> tracksId)
        {
            try
            {
                logger.Info("[userId:{0}]AddTracks", userId);
                var result = _httpWebBuilder
                            .Post(GetTracksUri(accessToken, playlistId, tracksId), userId)
                            .GetResponse();

                if (result != "true")
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<DeezerTrack>(result);

                return new DeezerTrack { Success = true };
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            
        }

        private string GetPlaylistUri(int userId, string accessToken, string title)
        {
            return string.Format("{0}user/{1}/playlists?access_token={2}&title={3}", 
                baseUri, userId, accessToken, title);
        }

        private string GetTracksUri(string accessToken, string playlistId, IEnumerable<string> tracksId)
        {
            return string.Format("{0}playlist/{1}/tracks?access_token={2}&songs={3}", 
                baseUri, playlistId, accessToken, string.Join(",", tracksId));
        }
    }
}
