using System.Net;
using FluentAssertions;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;

namespace Softcrafts.Jobs.Tests
{
    [TestFixture]
    public class OneApiTests
    {
        [Test]
        public void HeartbeatWithDatabaseTest()
        {
            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("/Admin/Heartbeat", Method.POST);
            request.AddQueryParameter("inklusiveDatabaseCheck", "true");

            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void HeartbeatWithoutDatabaseTest()
        {
            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("/Admin/Heartbeat", Method.POST);
            request.AddQueryParameter("inklusiveDatabaseCheck", "false");

            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void GetAllApiUsersTest()
        {
            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("/Admin/GetAllApiUsers", Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void GetAllAdminApiUsersTest()
        {
            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("/Admin/GetAllAdminApiUsers", Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ErmittelAlleJobsTest()
        {
            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("/Jobs/ErmittleAlleJobs", Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Test]
        public void CountAlleTest()
        {
            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("/Jobs/CountAlle", Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void CountOffenTest()
        {
            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("/Jobs/CountOffen", Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void CountCanceledTest()
        {
            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("/Jobs/CountCanceled", Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ErmittleUnerledigteJobsTest()
        {
            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("/Jobs/ErmittleUnerledigteJobs", Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ErmittleGecancelteJobsTest()
        {
            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("/Jobs/ErmittleGecancelteJobs", Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }



    }
}
