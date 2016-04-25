using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Gateway.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Lang { get; set; }
        public string Birthday { get; set; }
        public string AccessToken { get; set; }
    }
}
