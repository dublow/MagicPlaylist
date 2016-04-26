using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using NLog;
using MagicPlaylist.Common.Profilers;
using MagicPlaylist.Common.Loggers;
using System;

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
                using (var smartTimer = new SmartTimer((x, u) => GatewayLoggerInfo("Exit GetRandomTracks", userId, x.Elapsed)))
                {
                    GatewayLoggerInfo("GetRandomTracks", userId);
                    
                    using (var connection = _provider.Create())
                    {
                        var tracks = connection
                                .Query<string>("track.GetRandom", new { total = 10 },
                                commandType: CommandType.StoredProcedure);

                        return tracks ?? Enumerable.Empty<string>();
                    }
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        private void GatewayLoggerInfo(string message)
        {
            logger.Info(LoggerCreator.Info("Gateway", message));
        }

        private void GatewayLoggerInfo(string message, TimeSpan elasped)
        {
            logger.Info(LoggerCreator.Info("Gateway", message, elasped));
        }

        private void GatewayLoggerInfo(string message, int userId)
        {
            logger.Info(LoggerCreator.Info("Gateway", message, userId));
        }
        private void GatewayLoggerInfo(string message, int userId, TimeSpan elasped)
        {
            logger.Info(LoggerCreator.Info("Gateway", message, userId, elasped));
        }
    }
}
