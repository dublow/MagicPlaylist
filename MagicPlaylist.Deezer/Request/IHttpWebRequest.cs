using System.IO;
using System.Net;

namespace MagicPlaylist.Deezer.Request
{
    public interface IHttpWebRequest
    {
        int UserId { get; }
        string Method { get; set; }
        string ContentType { get; set; }
        long ContentLength { get; set; }
        void Create(string uri, int userId);
        Stream GetRequestStream();
        WebResponse GetResponse();
    }
}
