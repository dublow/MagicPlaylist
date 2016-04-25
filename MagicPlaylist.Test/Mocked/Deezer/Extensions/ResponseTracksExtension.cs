using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Test.Mocked.Deezer.Extensions
{
    public static class ResponseTracksExtension
    {
        public static MockedHttpWebRequest SuccessTracksResponse(this MockedHttpWebRequest mockedHttpWebRequest)
        {
            return mockedHttpWebRequest.SetResponseTracks("true");
        }

        public static MockedHttpWebRequest ErrorTracksInvalidPlaylistIdResponse(this MockedHttpWebRequest mockedHttpWebRequest)
        {
            return mockedHttpWebRequest.SetResponseTracks("{'error':{'type':'DataException','message':'no data','code':800}}");
        }

        public static MockedHttpWebRequest ErrorTracksInvalidTrackIdResponse(this MockedHttpWebRequest mockedHttpWebRequest)
        {
            return mockedHttpWebRequest.SetResponseTracks("{ 'error':{ 'type':'ParameterException','message':'Wrong parameter: songs','code':500} }");
        }
    }
}
