using MagicPlaylist.Deezer.Request;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Test.Mocked.Deezer
{
    public class MockedHttpWebRequest
    {
        private readonly Mock<IHttpWebRequest> _mockHttpWebRequest;
        public string uri;
        private Queue<MemoryStream> _queueStream;

        private MockedHttpWebRequest()
        {
            _mockHttpWebRequest = new Mock<IHttpWebRequest>();
            _mockHttpWebRequest.Setup(x => x.GetRequestStream()).Returns(new MemoryStream());
            _queueStream = new Queue<MemoryStream>();
        }

        public static MockedHttpWebRequest Create()
        {
            return new MockedHttpWebRequest();
        }

        public MockedHttpWebRequest SetCreate(string uri)
        {
            this.uri = uri;
            return this;
        }

        public MockedHttpWebRequest SetResponsePlaylist(string data)
        {
            var byteDatas = Encoding.UTF8.GetBytes(data);

            var playlistStream = new MemoryStream();
            playlistStream.Write(byteDatas, 0, byteDatas.Length);
            playlistStream.Position = 0;
            playlistStream.Flush();

            _queueStream.Enqueue(playlistStream);
            return this;
        }

        public MockedHttpWebRequest SetResponseTracks(string data)
        {
            var byteDatas = Encoding.UTF8.GetBytes(data);

            var tracksStream = new MemoryStream();
            tracksStream.Write(byteDatas, 0, byteDatas.Length);
            tracksStream.Position = 0;
            tracksStream.Flush();

            _queueStream.Enqueue(tracksStream);
            return this;
        }

        public MockedHttpWebRequest NullResponse()
        {
            _mockHttpWebRequest.Setup(x => x.GetResponse()).Returns((WebResponse)null);

            return this;
        }

        public IHttpWebRequest Build()
        {
            SetResponse();
            return _mockHttpWebRequest.Object;
        }
        
        private void SetResponse()
        {
            if (_queueStream.Any())
            {
                var mockWebResponse = new Mock<WebResponse>();
                mockWebResponse.Setup(x => x.GetResponseStream()).Returns(_queueStream.Dequeue);
                _mockHttpWebRequest.Setup(x => x.GetResponse()).Returns(mockWebResponse.Object);
            }
        }
    }
}
