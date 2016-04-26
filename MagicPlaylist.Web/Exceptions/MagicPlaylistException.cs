using System;

namespace MagicPlaylist.Web.Exceptions
{
    public class MagicPlaylistException : Exception
    {
        private readonly int userId;
        public MagicPlaylistException(string message) : base(message)
        {

        }

        public MagicPlaylistException(int userId, string message) : base(message)
        {
            this.userId = userId;
        }

        public override string ToString()
        {
            return string.Format("[userId:{0}]{1}", this.userId, this.Message);
        }
    }
}
