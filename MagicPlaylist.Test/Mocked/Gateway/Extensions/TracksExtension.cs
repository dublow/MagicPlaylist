using MagicPlaylist.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Test.Mocked.Gateway.Extensions
{
    public static class TracksExtension
    {
        public static MockedRadioGateway TracksGenerator(this MockedRadioGateway mockedRadioGateway, int total)
        {
            var result = from item in Enumerable.Range(0, total)
                         select GenerateTrackId();

            return mockedRadioGateway.SetTracks(result);
        }

        private static string GenerateTrackId()
        {
            return Enumerable
                        .Range(0, 5)
                        .Aggregate(new StringBuilder(), (builder, value) => 
                            builder.Append(RandomNumber.Between(0, 9)), builder => builder.ToString());
        }
    }

    
}
