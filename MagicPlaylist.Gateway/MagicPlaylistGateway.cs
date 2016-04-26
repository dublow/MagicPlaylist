using Dapper;
using System.Data;
using MagicPlaylist.Gateway.Models;
using NLog;
using System.Linq;

namespace MagicPlaylist.Gateway
{
    public interface IMagicPlaylistGateway
    {
        bool CanAddPlaylist(int userId);
        void AddOrUpdateUser(UserModel user);
        void AddError(ErrorModel error);
    }
    public class MagicPlaylistGateway : IMagicPlaylistGateway
    {
        private static Logger logger = LogManager.GetLogger("Gateway");

        private readonly IProvider _provider;
        public MagicPlaylistGateway(IProvider provider)
        {
            this._provider = provider;
        }

        public bool CanAddPlaylist(int userId)
        {
            try
            {
                logger.Info("[userId:{0}]CanAddPlaylist", userId);
                using (var connection = _provider.Create())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userId", userId);
                    parameters.Add("@canAdd", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    connection.Execute("user.CanAddPlaylist", parameters, commandType: CommandType.StoredProcedure);

                    return parameters.Get<bool>("@canAdd");
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
        public void AddOrUpdateUser(UserModel user)
        {
            try
            {
                logger.Info("[userId:{0}]AddOrUpdateUser", user.Id);
                using (var connection = _provider.Create())
                {
                    connection.Execute("user.AddOrUpdate", new
                    {
                        Id = user.Id,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Email = user.Email,
                        Gender = user.Gender,
                        Name = user.Name,
                        Country = user.Country,
                        Lang = user.Lang,
                        Birthday = user.Birthday,
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public void AddError(ErrorModel error)
        {
            try
            {
                logger.Info("[type:{0}]AddError", error.errorType);
                using (var connection = _provider.Create())
                {
                    connection.Execute("log.AddError", new
                    {
                        ErrorType = error.errorType,
                        Message = error.message,
                        StackTrace = error.stackTrace
                    }, commandType: CommandType.StoredProcedure);
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
