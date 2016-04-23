using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Deezer
{
    public class HttpWebBuilder
    {
        private readonly HttpWebRequest _httpWebRequest;
        private HttpWebBuilder(string uri, string method)
        {
            _httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            _httpWebRequest.Method = method;
        }

        public static HttpWebBuilder Post(string uri)
        {
            return new HttpWebBuilder(uri, "POST");
        }

        public static HttpWebBuilder Get(string uri)
        {
            return new HttpWebBuilder(uri, "Get");
        }

        public HttpWebBuilder WithBody(string data, string contentType)
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

        public string GetResponse()
        {
            using (var webResponse = _httpWebRequest.GetResponse())
            {
                using (var streamResponse = new StreamReader(webResponse.GetResponseStream()))
                {
                    return streamResponse.ReadToEnd();
                }
            }
        }
    }
}
