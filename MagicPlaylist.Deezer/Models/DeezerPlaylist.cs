namespace MagicPlaylist.Deezer.Models
{
    public class DeezerPlaylist
    {
        public string PlaylistUrl
        {
            get
            {
                return string.Format("https://www.deezer.com/playlist/{0}", Id);
            }
        }
        public string Id { get; set; }
        public Error Error { get; set; }
        public bool HasError { get { return Error != null; } }
    }
}
