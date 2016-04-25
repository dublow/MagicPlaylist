using MagicPlaylist.Deezer.Builder;
using MagicPlaylist.Deezer.Models;
using System.Collections.Generic;

namespace MagicPlaylist.Deezer
{
    public class HttpDeezer
    {
        private const string baseUri = "http://api.deezer.com/";
        private readonly HttpWebBuilder _httpWebBuilder;
        public HttpDeezer(HttpWebBuilder httpWebBuilder)
        {
            _httpWebBuilder = httpWebBuilder;
        }

        public DeezerPlaylist AddPlaylist(string userId, string accessToken, string title)
        {
            var result = _httpWebBuilder
                            .Post(GetPlaylistUri(userId, accessToken, title))
                            .GetReponseToJson<DeezerPlaylist>();

            return result;
        }

        public DeezerTrack AddTracks(string accessToken, string playlistId, IEnumerable<string> tracksId)
        {
            var result = _httpWebBuilder
                            .Post(GetTracksUri(accessToken, playlistId, tracksId))
                            .GetResponse();

            if (result != "true")
                return Newtonsoft.Json.JsonConvert.DeserializeObject<DeezerTrack>(result);

            return new DeezerTrack { Success = true };
        }

        private string GetPlaylistUri(string userId, string accessToken, string title)
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
