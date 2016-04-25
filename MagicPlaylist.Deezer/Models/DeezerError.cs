namespace MagicPlaylist.Deezer.Models
{
    public class DeezerError
    {
        public Error Error { get; set; }
    }

    public class Error
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
    }
}
