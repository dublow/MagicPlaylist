using MagicPlaylist.Deezer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Deezer
{
    public class HttpDeezer
    {
        private const string baseUri = "http://api.deezer.com/";

        private readonly string _accessToken;
        private readonly string _userId;

        private HttpDeezer(string accessToken, string userId)
        {
            this._accessToken = accessToken;
            this._userId = userId;
        }

        public static HttpDeezer Create(string accessToken, string userId)
        {
            return new HttpDeezer(accessToken, userId);
        }

        public string AddPlaylist(string title)
        {
            var result = HttpWebBuilder.Post(GetPlaylistUri(title)).GetResponse();
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DeezerPlaylist>(result);
            return obj.Id;
        }

        public string AddTracks(string playlistId, IEnumerable<string> tracksId)
        {
            return HttpWebBuilder.Post(GetTracksUri(playlistId, tracksId)).GetResponse();
        }

        private string GetPlaylistUri(string title)
        {
            return string.Format("{0}user/{1}/playlists?access_token={2}&title={3}", baseUri, _userId, _accessToken, title);
        }

        private string GetTracksUri(string playlistId, IEnumerable<string> tracksId)
        {
            var j = string.Join(",", tracksId);
            return string.Format("{0}playlist/{1}/tracks?access_token={2}&songs={3}", baseUri, playlistId, _accessToken, string.Join(",", tracksId));
        }
    }
}
