using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using NLog;

namespace MagicPlaylist.Gateway
{
    public interface IRadioGateway
    {
        IEnumerable<string> GetRandomTracks(int userId);
    }
    public class RadioGateway : IRadioGateway
    {
        private static Logger logger = LogManager.GetLogger("Gateway");

        private readonly IProvider _provider;
        public RadioGateway(IProvider provider)
        {
            this._provider = provider;
        }
        public IEnumerable<string> GetRandomTracks(int userId)
        {
            try
            {
                logger.Info("GetRandomTracks[userId:{0}]", userId);
                using (var connection = _provider.Create())
                {
                    var tracks = connection
                            .Query<string>("track.GetRandom", new { total = 10 },
                            commandType: CommandType.StoredProcedure);

                    return tracks ?? Enumerable.Empty<string>();
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            
        }
    }
}
