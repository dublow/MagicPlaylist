namespace MagicPlaylist.Deezer.Builder
{
    public interface IHttpWebBuilder
    {
        IHttpWebBuilder Post(string uri, int userId);
        IHttpWebBuilder Get(string uri, int userId);
        IHttpWebBuilder WithBody(string data, string contentType);
        T GetReponseToJson<T>();
        string GetResponse();
    }
}
