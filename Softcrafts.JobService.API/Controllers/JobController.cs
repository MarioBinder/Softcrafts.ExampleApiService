using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Softcrafts.Jobs.Business;
using Softcrafts.Jobs.Entities;
using Softcrafts.Jobs.Entities.Base;
using Softcrafts.JobService.API.Models;
using Action = Softcrafts.Jobs.Entities.Base.Action;
#pragma warning disable 4014

namespace Softcrafts.jobservice.API.Controllers
{
    /// <summary>
    /// CRUD SEMAS Job API
    /// </summary>
    public partial class JobController : JobBaseController
    {
        private readonly string _username;
        /// <summary>
        /// CRUD JobController
        /// </summary>
        public JobController()
        {
            _username = GetCurrentApiUser();
        }
        private Link GetJobRootLinks()
        {
            return
                new Link()
                {
                    _infos = new Dictionary<string, string>()
                    {
                        {"_self", Request?.RequestUri?.AbsoluteUri},
                        {Names.CountAlle, GetRootUrl() + Names.GetJobCompleteJobNamePath(Names.CountAlle)},
                        {Names.CountCanceled, GetRootUrl() +Names.GetJobCompleteJobNamePath( Names.CountCanceled)},
                        {Names.CountInArbeit, GetRootUrl() + Names.GetJobCompleteJobNamePath(Names.CountInArbeit)},
                        {Names.CountOffen, GetRootUrl() + Names.GetJobCompleteJobNamePath(Names.CountOffen)}
                    }
                };
        }

        /// <summary>
        /// Ermittelt alle Jobs
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Die Authentifizierung über Basic Auth ist erforderlich
        /// #### C# Examples ####
        /// <!-- 
        /// <code>
        ///       using System.Net;
        ///       using FluentAssertions;
        ///       using NUnit.Framework;
        ///       using RestSharp;
        ///       using RestSharp.Authenticators;
        ///       [Test]
        ///       public void ErmittelAlleJobsTest()
        ///       {
        ///           var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///           client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///           var request = new RestRequest("/Jobs/ErmittleAlleJobs", Method.GET);
        ///           request.RequestFormat = DataFormat.Json;
        ///           IRestResponse response = client.Execute(request);
        ///           response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///        }
        /// </code>
        /// -->
        /// </remarks>
        /// <param name="pageNo">default = 1</param>
        /// <param name="pageSize">default = 10</param>
        [AllowAnonymous]
        [HttpGet, Route(Names.Jobs + "/" + Names.ErmittleAlleJobs, Name = Names.ErmittleAlleJobs)]
        public async Task<IHttpActionResult> ErmittleAlleJobs(int pageNo = 1, int pageSize = 10)
        {
            try
            {
                IHandleJobs repo = new JobRepository(_username);
                var result = await repo.GetAllJobs(c => !c.IsDeleted);

                int skip = (pageNo - 1) * pageSize;
                int total = result.Data.Count;

                result.Data = result.Data.Skip(skip).Take(pageSize).ToList();

                foreach (var job in result.Data)
                {
                    job._links = new Link()
                    {
                        _actions = new List<Action>()
                        {
                            CreateActionLink<JobDetailController>(Names.JobDetails, c=> c.JobDetails( job.UniqueId), Names.JobDetails),
                            CreateActionLink<JobDetailController>(Names.LoescheJob, c=> c.LoescheJob( job.UniqueId), Names.LoescheJob),
                            CreateActionLink<JobDetailController>(Names.SetzeJobInWork, c=> c.SetzeJobInWork( job.UniqueId), Names.SetzeJobInWork),
                            CreateActionLink<JobDetailController>(Names.SetzeJobDone, c=> c.SetzeJobDone( job.UniqueId), Names.SetzeJobDone),
                            CreateActionLink<JobDetailController>(Names.SetzeJobIsCanceled, c=> c.SetzeJobIsCanceled( job.UniqueId), Names.SetzeJobIsCanceled),
                        }
                    };
                }

                var linkBuilderResult = new PageLinkBuilder(Url, Names.ErmittleAlleJobs, null, pageNo, pageSize, total);
                return Ok(new JobResult<List<Job>>()
                {
                    SemasResult = result,
                    _links = GetJobRootLinks(),
                    _paging = new
                    {
                        First = linkBuilderResult.FirstPage,
                        Previous = linkBuilderResult.PreviousPage,
                        Next = linkBuilderResult.NextPage,
                        Last = linkBuilderResult.LastPage,
                        TotalItems = total,
                        TotalPageCount = linkBuilderResult.TotalPageCount
                    }
                });
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Ermittelt die Anzahl aller Jobs
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Die Authentifizierung über Basic Auth ist erforderlich
        /// #### C# Examples ####
        /// <!-- 
        /// <code>
        ///       using System.Net;
        ///       using FluentAssertions;
        ///       using NUnit.Framework;
        ///       using RestSharp;
        ///       using RestSharp.Authenticators;
        ///        [Test]
        ///        public void CountAlleTest()
        ///        {
        ///            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///            client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///            var request = new RestRequest("/Jobs/CountAlle", Method.GET);
        ///            request.RequestFormat = DataFormat.Json;
        ///            IRestResponse response = client.Execute(request);
        ///            response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///        }
        /// </code>
        /// -->
        /// </remarks>
        /// <returns></returns>
        [HttpGet, Route(Names.Jobs + "/" + Names.CountAlle, Name = Names.CountAlle)]
        public async Task<IHttpActionResult> CountAlle()
        {
            IHandleJobs repo = new JobRepository(_username);
            var result = await repo.GetAllJobs(c => true);
            return Ok(result.Data.Count);
        }

        /// <summary>
        /// Ermittelt die Anzahl aller offenen Jobs
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Die Authentifizierung über Basic Auth ist erforderlich
        /// #### C# Examples ####
        /// <!-- 
        /// <code>
        ///       using System.Net;
        ///       using FluentAssertions;
        ///       using NUnit.Framework;
        ///       using RestSharp;
        ///       using RestSharp.Authenticators;
        ///        [Test]
        ///        public void CountOffenTest()
        ///        {
        ///            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///            client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///            var request = new RestRequest("/Jobs/CountOffen", Method.GET);
        ///            request.RequestFormat = DataFormat.Json;
        ///            IRestResponse response = client.Execute(request);
        ///            response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///        }
        /// </code>
        /// -->
        /// </remarks>
        /// <returns></returns>
        [HttpGet, Route(Names.Jobs + "/" + Names.CountOffen, Name = Names.CountOffen)]
        public async Task<IHttpActionResult> CountOffen()
        {
            IHandleJobs repo = new JobRepository(_username);
            var result = await repo.GetAllJobs(c => !c.InWork && !c.Done && !c.IsCanceled);
            return Ok(result.Data.Count);
        }
        /// <summary>
        /// Ermittelt die Anzahl aller Jobs die aktuell in Arbeit sind.
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Die Authentifizierung über Basic Auth ist erforderlich
        /// #### C# Examples ####
        /// <!-- 
        /// <code>
        ///       using System.Net;
        ///       using FluentAssertions;
        ///       using NUnit.Framework;
        ///       using RestSharp;
        ///       using RestSharp.Authenticators;
        ///       [Test]
        ///       public void CountCanceledTest()
        ///       {
        ///           var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///           client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///           var request = new RestRequest("/Jobs/CountCanceled", Method.GET);
        ///           request.RequestFormat = DataFormat.Json;
        ///           IRestResponse response = client.Execute(request);
        ///           response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///       }
        /// </code>
        /// -->
        /// </remarks>
        /// <returns></returns>
        [HttpGet, Route(Names.Jobs + "/" + Names.CountInArbeit, Name = Names.CountInArbeit)]
        public async Task<IHttpActionResult> CountInArbeit()
        {
            IHandleJobs repo = new JobRepository(_username);
            var result = await repo.GetAllJobs(c => c.InWork && !c.Done && !c.IsCanceled);
            return Ok(result.Data.Count);
        }

        /// <summary>
        /// Ermittelt die Anzahl aller Jobs die gecanceled sind.
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Die Authentifizierung über Basic Auth ist erforderlich
        /// #### C# Examples ####
        /// <!-- 
        /// <code>
        ///       using System.Net;
        ///       using FluentAssertions;
        ///       using NUnit.Framework;
        ///       using RestSharp;
        ///       using RestSharp.Authenticators;
        ///       [Test]
        ///       public void ErmittleGecancelteJobsTest()
        ///       {
        ///           var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///           client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///           var request = new RestRequest("/Jobs/ErmittleGecancelteJobs", Method.GET);
        ///           request.RequestFormat = DataFormat.Json;
        ///           IRestResponse response = client.Execute(request);
        ///           response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///       }
        /// </code>
        /// -->
        /// </remarks>
        /// <returns></returns>
        [HttpGet, Route(Names.Jobs + "/" + Names.CountCanceled, Name = Names.CountCanceled)]
        public async Task<IHttpActionResult> CountCanceled()
        {
            IHandleJobs repo = new JobRepository(_username);
            var result = await repo.GetAllJobs(c => c.IsCanceled);
            return Ok(result.Data.Count);
        }



        /// <summary>
        /// Ermittelt Jobs die noch unerledigt sind.
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Die Authentifizierung über Basic Auth ist erforderlich
        /// #### C# Examples ####
        /// <!-- 
        /// <code>
        ///       using System.Net;
        ///       using FluentAssertions;
        ///       using NUnit.Framework;
        ///       using RestSharp;
        ///       using RestSharp.Authenticators;
        ///        [Test]
        ///        public void ErmittleUnerledigteJobsTest()
        ///        {
        ///            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///            client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///            var request = new RestRequest("/Jobs/ErmittleUnerledigteJobs", Method.GET);
        ///            request.RequestFormat = DataFormat.Json;
        ///            IRestResponse response = client.Execute(request);
        ///            response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///        }
        /// </code>
        /// -->
        /// </remarks>
        /// <returns></returns>
        [HttpGet, Route(Names.Jobs + "/" + Names.ErmittleUnerledigteJobs, Name = Names.ErmittleUnerledigteJobs)]
        public async Task<IHttpActionResult> ErmittleUnerledigteJobs()
        {
            try
            {
                IHandleJobs repo = new JobRepository(_username);
                var result = await repo.GetNextOpenJobs();
                foreach (var job in result.Data)
                {
                    job._links = new Link()
                    {
                        _actions = new List<Action>()
                        {

                            CreateActionLink<JobController>("_self", c=> c.ErmittleUnerledigteJobs(), Names.ErmittleUnerledigteJobs),
                            CreateActionLink<JobDetailController>(Names.SetzeJobInWork, c=> c.SetzeJobInWork( job.UniqueId), Names.SetzeJobInWork),
                            CreateActionLink<JobDetailController>(Names.SetzeJobDone, c=> c.SetzeJobDone( job.UniqueId), Names.SetzeJobDone),
                            CreateActionLink<JobDetailController>(Names.SetzeJobIsCanceled, c=> c.SetzeJobIsCanceled( job.UniqueId), Names.SetzeJobIsCanceled),
                        }
                    };
                }

                var links = GetJobRootLinks();
                links._infos = new Dictionary<string, string> { { "_dataCount", result.Data?.Count.ToString() } }
                    .Concat(links._infos).ToDictionary(k => k.Key, v => v.Value);

                return Ok(new JobResult<List<Job>>()
                {
                    SemasResult = result,
                    _links = links
                });
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Ermittelt Jobs die gecanceled wurden.
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Die Authentifizierung über Basic Auth ist erforderlich
        /// #### C# Examples ####
        /// <!-- 
        /// <code>
        ///       using System.Net;
        ///       using FluentAssertions;
        ///       using NUnit.Framework;
        ///       using RestSharp;
        ///       using RestSharp.Authenticators;
        ///        [Test]
        ///        public void ErmittleGecancelteJobsTest()
        ///        {
        ///            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///            client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///            var request = new RestRequest("/Jobs/ErmittleGecancelteJobs", Method.GET);
        ///            request.RequestFormat = DataFormat.Json;
        ///            IRestResponse response = client.Execute(request);
        ///            response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///        }
        /// </code>
        /// -->
        /// </remarks>
        /// <returns></returns>
        [HttpGet, Route(Names.Jobs + "/" + Names.ErmittleGecancelteJobs, Name = Names.ErmittleGecancelteJobs)]
        public async Task<IHttpActionResult> ErmittleGecancelteJobs()
        {
            try
            {
                IHandleJobs repo = new JobRepository(_username);
                var result = await repo.GetCanceledJobs();
                foreach (var job in result.Data)
                {
                    job._links = new Link()
                    {
                        _actions = new List<Action>()
                        {
                            CreateActionLink<JobDetailController>(Names.LoescheJobIsCanceledFlag, c=> c.LoescheJobIsCanceledFlag( job.UniqueId), Names.LoescheJobIsCanceledFlag)
                        }
                    };
                }

                var links = GetJobRootLinks();
                links._infos = new Dictionary<string, string> { { "_dataCount", result.Data?.Count.ToString() } }
                    .Concat(links._infos).ToDictionary(k => k.Key, v => v.Value);

                return Ok(new JobResult<List<Job>>()
                {
                    SemasResult = result,
                    _links = links
                });
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }


        /// <summary>
        /// Speichert einen neuen Job
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [HttpPost, Route(Names.Jobs + "/" + Names.NeuerJob, Name = Names.NeuerJob)]
        public async Task<IHttpActionResult> NeuerJob(Job job)
        {
            if (job == null)
                return Content(HttpStatusCode.BadRequest, "Job may not be null or empty.");

            IHandleJobs repo = new JobRepository(_username);
            var result = await repo.SaveJob(job);

            result.Data._links = new Link()
            {
                _actions = new List<Action>()
                {
                    CreateActionLink<JobDetailController>(Names.JobDetails, c=> c.JobDetails( job.UniqueId), Names.JobDetails),
                    CreateActionLink<JobDetailController>(Names.LoescheJob, c=> c.LoescheJob( job.UniqueId), Names.LoescheJob),
                    CreateActionLink<JobDetailController>(Names.SetzeJobInWork, c=> c.SetzeJobInWork( job.UniqueId), Names.SetzeJobInWork),
                    CreateActionLink<JobDetailController>(Names.SetzeJobDone, c=> c.SetzeJobDone( job.UniqueId), Names.SetzeJobDone),
                    CreateActionLink<JobDetailController>(Names.SetzeJobIsCanceled, c=> c.SetzeJobIsCanceled( job.UniqueId), Names.SetzeJobIsCanceled),
                }
            };

            var links = GetJobRootLinks();
            return Ok(new JobResult<Job>()
            {
                SemasResult = result,
                _links = links
            });
        }
    }
}
