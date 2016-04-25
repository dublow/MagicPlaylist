namespace MagicPlaylist.Deezer.Models
{
    public class DeezerPlaylist
    {
        public string Id { get; set; }
        public Error Error { get; set; }
        public bool HasError { get { return Error != null; } }
    }
}
