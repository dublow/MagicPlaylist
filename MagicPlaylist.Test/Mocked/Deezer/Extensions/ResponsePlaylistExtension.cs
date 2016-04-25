using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Test.Mocked.Deezer.Extensions
{
    public static class ResponsePlaylistExtension
    {
        public static MockedHttpWebRequest SuccessPlaylistResponse(this MockedHttpWebRequest mockedHttpWebRequest)
        {
            return mockedHttpWebRequest.SetResponsePlaylist("{id:12345}");
        }

        public static MockedHttpWebRequest ErrorPlaylistResponse(this MockedHttpWebRequest mockedHttpWebRequest)
        {
            return mockedHttpWebRequest.SetResponsePlaylist("{'error':{'type':'ParameterException','message':'Wrong parameter: playlistid','code':500}}");
        }
    }
}
