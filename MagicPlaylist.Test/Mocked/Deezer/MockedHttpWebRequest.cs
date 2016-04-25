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

        private MockedHttpWebRequest()
        {
            _mockHttpWebRequest = new Mock<IHttpWebRequest>();
            _mockHttpWebRequest.Setup(x => x.GetRequestStream()).Returns(new MemoryStream());
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

        public MockedHttpWebRequest SetResponse(string data)
        {
            var byteDatas = Encoding.UTF8.GetBytes(data);

            var stream = new MemoryStream();
            stream.Write(byteDatas, 0, byteDatas.Length);
            stream.Flush();
            stream.Position = 0;

            var mockWebResponse = new Mock<WebResponse>();
            mockWebResponse.Setup(x => x.GetResponseStream()).Returns(stream);
            _mockHttpWebRequest.Setup(x => x.GetResponse()).Returns(mockWebResponse.Object);

            return this;
        }

        public MockedHttpWebRequest NullResponse()
        {
            _mockHttpWebRequest.Setup(x => x.GetResponse()).Returns((WebResponse)null);

            return this;
        }

        public IHttpWebRequest Build()
        {
            return _mockHttpWebRequest.Object;
        }
    }
}
