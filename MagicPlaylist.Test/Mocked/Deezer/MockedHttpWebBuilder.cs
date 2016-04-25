using MagicPlaylist.Deezer.Builder;
using Moq;

namespace MagicPlaylist.Test.Mocked.Deezer
{
    public class MockedHttpWebBuilder
    {
        private readonly Mock<IHttpWebBuilder> _mockHttpWebBuilder;
        public string postUri;
        public string getUri;
        public string data;
        public string contentType;

        private MockedHttpWebBuilder()
        {
            _mockHttpWebBuilder = new Mock<IHttpWebBuilder>();
        }

        public static MockedHttpWebBuilder Create()
        {
            return new MockedHttpWebBuilder();
        }

        public MockedHttpWebBuilder SetPost(string uri)
        {
            postUri = uri;
            return this;
        }

        public MockedHttpWebBuilder SetGet(string uri)
        {
            getUri = uri;
            return this;
        }

        public MockedHttpWebBuilder SetBody(string data, string contentType)
        {
            this.data = data;
            this.contentType = contentType;
            return this;
        }

        public MockedHttpWebBuilder SetResponse(string data)
        {
            _mockHttpWebBuilder.Setup(x => x.GetResponse()).Returns(data);
            return this;
        }

        public MockedHttpWebBuilder SetResponseJson<T>(string data)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
            _mockHttpWebBuilder.Setup(x => x.GetReponseToJson<T>()).Returns(obj);
            return this;
        }

        public IHttpWebBuilder Build()
        {
            return _mockHttpWebBuilder.Object;
        }

    }
}
