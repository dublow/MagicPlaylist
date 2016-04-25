﻿using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;

namespace MagicPlaylist.Gateway
{
    public interface IRadioGateway
    {
        IEnumerable<string> GetRandomTracks();
    }
    public class RadioGateway : IRadioGateway
    {
        private readonly IProvider _provider;
        public RadioGateway(IProvider provider)
        {
            this._provider = provider;
        }
        public IEnumerable<string> GetRandomTracks()
        {
            using (var connection = _provider.Create())
            {
                var tracks = connection
                        .Query<string>("track.GetRandom", new { total = 10}, 
                        commandType: CommandType.StoredProcedure);

                return tracks ?? Enumerable.Empty<string>();
            }
        }
    }
}
