using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace MagicPlaylist.Gateway
{
    public interface IMagicPlaylistGateway
    {
        void AddOrUpdateUser(int id, string firstname, string lastname, string email, 
            string gender, string name, string country, string lang, string birthday);
        void AddError(string errorType, string message, string stackTrace);
    }
    public class MagicPlaylistGateway : IMagicPlaylistGateway
    {
        private readonly IProvider _provider;
        public MagicPlaylistGateway(IProvider provider)
        {
            this._provider = provider;
        }

        public void AddOrUpdateUser(int id, string firstname, string lastname, string email,
            string gender, string name, string country, string lang, string birthday)
        {
            using (var connection = _provider.Create())
            {
                connection.Execute("user.AddOrUpdate", new
                    {
                        Id = id,
                        Firstname = firstname,
                        Lastname = lastname,
                        Email = email,
                        Gender = gender,
                        Name = name,
                        Country = country,
                        Lang = lang,
                        Birthday = birthday,
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddError(string errorType, string message, string stackTrace)
        {
            using (var connection = _provider.Create())
            {
                connection.Execute("log.AddError", new
                {
                   ErrorType = errorType,
                   Message = message,
                   StackTrace = stackTrace
                }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
