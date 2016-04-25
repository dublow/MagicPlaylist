using System.Data;

namespace MagicPlaylist.Gateway
{
    public interface IProvider
    {
        IDbConnection Create();
    }
}
