using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Deezer.Models
{
    public class DeezerError
    {
        public Error Error { get; set; }
    }

    public class Error
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
    }

    //{"error":{"type":"ParameterException","message":"Wrong parameter: songs","code":500}}
}
