using MagicPlaylist.Deezer.Request;
using System.IO;
using System.Text;

namespace MagicPlaylist.Deezer.Builder
{
    public class HttpWebBuilder : IHttpWebBuilder
    {
        private readonly IHttpWebRequest _httpWebRequest;
        public HttpWebBuilder(IHttpWebRequest httpWebRequest)
        {
            _httpWebRequest = httpWebRequest;
        }

        public IHttpWebBuilder Post(string uri)
        {
            _httpWebRequest.Create(uri);
            _httpWebRequest.Method = "POST";
            return this;
        }

        public IHttpWebBuilder Get(string uri)
        {
            _httpWebRequest.Create(uri);
            _httpWebRequest.Method = "GET";
            return this;
        }

        public IHttpWebBuilder WithBody(string data, string contentType)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var bytesLength = bytes.Length;

            _httpWebRequest.ContentLength = bytesLength;
            _httpWebRequest.ContentType = contentType;

            var streamRequest = _httpWebRequest.GetRequestStream();
            streamRequest.Write(bytes, 0, bytesLength);
            streamRequest.Flush();

            return this;
        }

        public T GetReponseToJson<T>()
        {
            var result = GetResponse();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
        }
        public string GetResponse()
        {
            using (var webResponse = _httpWebRequest.GetResponse())
            {
                if (webResponse == null)
                    return string.Empty;
                using (var streamResponse = new StreamReader(webResponse.GetResponseStream()))
                {
                    return streamResponse.ReadToEnd();
                }
            }
        }
    }
}
