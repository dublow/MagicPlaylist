namespace MagicPlaylist.Deezer.Models
{
    public class DeezerTrack
    {
        public bool Success { get; set; }
        public Error Error { get; set; }
        public bool HasError { get { return Error != null; } }
    }
}
