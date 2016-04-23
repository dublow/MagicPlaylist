using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;

namespace MagicPlaylist.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = new Uri("http://localhost:3000");
            using (var nh = new NancyHost(url))
            {
                nh.Start();
                Console.ReadLine();
            }
        }
    }
}
