using System.IO;
using System.Net;

namespace MagicPlaylist.Deezer.Request
{
    public interface IHttpWebRequest
    {
        string Method { get; set; }
        string ContentType { get; set; }
        long ContentLength { get; set; }
        void Create(string uri);
        Stream GetRequestStream();
        WebResponse GetResponse();
    }
}
