using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Gateway
{
    public class SqlProvider : IProvider
    {
        private readonly string _cnx;

        public SqlProvider(string cnx)
        {
            this._cnx = cnx;
        }
        public IDbConnection Create()
        {
            return new SqlConnection(_cnx);
        }
    }
}
