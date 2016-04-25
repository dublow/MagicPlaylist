namespace MagicPlaylist.Deezer.Builder
{
    public interface IHttpWebBuilder
    {
        IHttpWebBuilder Post(string uri);
        IHttpWebBuilder Get(string uri);
        IHttpWebBuilder WithBody(string data, string contentType);
        T GetReponseToJson<T>();
        string GetResponse();
    }
}
