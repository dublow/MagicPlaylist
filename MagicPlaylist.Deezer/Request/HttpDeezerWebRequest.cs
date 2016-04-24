using System.IO;
using System.Net;

namespace MagicPlaylist.Deezer.Request
{
    public class HttpDeezerWebRequest : IHttpWebRequest
    {
        private HttpWebRequest _httpWebRequest;
       
        public long ContentLength
        {
            get
            {
                return _httpWebRequest.ContentLength;
            }

            set
            {
                _httpWebRequest.ContentLength = value;
            }
        }

        public string ContentType
        {
            get
            {
                return _httpWebRequest.ContentType;
            }

            set
            {
                _httpWebRequest.ContentType = value;
            }
        }

        public string Method
        {
            get
            {
                return _httpWebRequest.Method;
            }

            set
            {
                _httpWebRequest.Method = value;
            }
        }

        public void Create(string uri)
        {
            _httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
        }

        public Stream GetRequestStream()
        {
            return _httpWebRequest.GetRequestStream();
        }

        public WebResponse GetResponse()
        {
            return _httpWebRequest.GetResponse();
        }
    }
}
