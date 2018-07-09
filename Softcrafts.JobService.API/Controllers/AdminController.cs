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
    /// API Admin Routes
    /// </summary>
    public partial class AdminController : JobBaseController
    {
        private readonly string _username;

        /// <summary>
        /// AdminController
        /// </summary>
        public AdminController()
        {
            _username = GetCurrentApiUser();
        }

        /// <summary>
        /// Prüft die API auf Funktionstüchtigkeit
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Eine Authentifizierung ist nicht erforderlich
        /// - inklusiveDatabaseCheck true|false
        /// #### C# Examples ####
        /// <!-- 
        /// <code>
        ///        using System.Net;
        ///        using FluentAssertions;
        ///        using NUnit.Framework;
        ///        using RestSharp;
        ///        using RestSharp.Authenticators;
        ///        [Test]
        ///        public void HeartbeatWithDatabaseTest()
        ///        {
        ///            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///            client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///            var request = new RestRequest("/Admin/Heartbeat", Method.POST);
        ///            request.AddQueryParameter("inklusiveDatabaseCheck", "true");
        ///            request.RequestFormat = DataFormat.Json;
        ///            IRestResponse response = client.Execute(request);
        ///            response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///        }
        ///        [Test]
        ///        public void HeartbeatWithoutDatabaseTest()
        ///        {
        ///            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///            client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///            var request = new RestRequest("/Admin/Heartbeat", Method.POST);
        ///            request.AddQueryParameter("inklusiveDatabaseCheck", "false");
        ///            request.RequestFormat = DataFormat.Json;
        ///            IRestResponse response = client.Execute(request);
        ///            response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///        }
        /// </code>
        /// -->
        /// </remarks>
        /// <param name="inklusiveDatabaseCheck">true|false</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route(Names.Admin + "/" + Names.Heartbeat, Name = Names.Heartbeat)]
        public IHttpActionResult Heartbeat(bool inklusiveDatabaseCheck)
        {
            IHandleJobs repo = new JobRepository(_username);
            return Ok(repo.HeartBeat(inklusiveDatabaseCheck));
        }

        /// <summary>
        /// Ermittelt alle API-User
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Die Authentifizierung über Basic Auth ist erforderlich
        /// - Erfordert die Admin-Rolle
        /// #### C# Examples ####
        /// <!-- 
        /// <code>
        ///       using System.Net;
        ///       using FluentAssertions;
        ///       using NUnit.Framework;
        ///       using RestSharp;
        ///       using RestSharp.Authenticators;
        ///       [Test]
        ///       public void GetAllApiUsersTest()
        ///       {
        ///           var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///           client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///           var request = new RestRequest("/Admin/GetAllApiUsers", Method.GET);
        ///           request.RequestFormat = DataFormat.Json;
        ///           IRestResponse response = client.Execute(request);
        ///           response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///       }
        /// </code>
        /// -->
        /// </remarks>
        /// <param name="pageNo">default = 1</param>
        /// <param name="pageSize">default = 10</param>
        [HttpGet, Route(Names.Admin + "/" + Names.GetAllApiUsers, Name = Names.GetAllApiUsers)]
        public async Task<IHttpActionResult> GetAllApiUsers(int pageNo = 1, int pageSize = 10)
        {
            return await GetAllApiUsersHelper(c => true, pageNo, pageSize);
        }

        /// <summary>
        /// Ermittelt alle APIUser in der Rolle Admin
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Die Authentifizierung über Basic Auth ist erforderlich
        /// - Erfordert die Admin-Rolle
        /// #### C# Examples ####
        /// <!-- 
        /// <code>
        ///       using System.Net;
        ///       using FluentAssertions;
        ///       using NUnit.Framework;
        ///       using RestSharp;
        ///       using RestSharp.Authenticators;
        ///        [Test]
        ///        public void GetAllAdminApiUsersTest()
        ///        {
        ///            var client = new RestClient("https://test.api.corpinter.net/mbd_semas_jobservice_api_2");
        ///            client.Authenticator = new HttpBasicAuthenticator("test", "test");
        ///            var request = new RestRequest("/Admin/GetAllAdminApiUsers", Method.GET);
        ///            request.RequestFormat = DataFormat.Json;
        ///            IRestResponse response = client.Execute(request);
        ///            response.StatusCode.Should().Be(HttpStatusCode.OK);
        ///        }
        /// </code>
        /// -->
        /// </remarks>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet, Route(Names.Admin + "/" + Names.GetAllAdminApiUsers, Name = Names.GetAllAdminApiUsers)]
        public async Task<IHttpActionResult> GetAllAdminApiUsers(int pageNo = 1, int pageSize = 10)
        {
            return await GetAllApiUsersHelper(c => c.IsAdmin, pageNo, pageSize);
        }

        /// <summary>
        ///  Ermittelt alle API-User
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private async Task<IHttpActionResult> GetAllApiUsersHelper(Func<APIUser, bool> predicate, int pageNo = 1, int pageSize = 10)
        {
            IHandleJobs repo = new JobRepository(_username);
            var dbResult = await repo.GetAllApiUsers(predicate);
            if (!dbResult.Success)
                return Content(HttpStatusCode.BadRequest, dbResult.Message);

            int skip = (pageNo - 1) * pageSize;
            int total = dbResult.Data.Count;

            dbResult.Data = dbResult.Data.Skip(skip).Take(pageSize).ToList();

            var result = dbResult.Data.Select(user => new
            {
                user.Username,
                user.IsAdmin,
                user.CreatedUser,
                user.CreatedDate,
                user.UniqueId,
                user.Id,
                _links = new Link()
                {
                    _actions = new List<Action>()
                    {
                        !user.IsAdmin ? CreateActionLink<AdminController>(Names.SetApiUserAsAdmin, c => c.SetApiUserAsAdmin(user.Username), Names.SetApiUserAsAdmin):null,
                        user.IsAdmin ? CreateActionLink<AdminController>(Names.RevokeAdminFromApiUser, c => c.RevokeAdminFromApiUser(user.Username), Names.RevokeAdminFromApiUser) : null,
                        CreateActionLink<AdminController>(Names.DeleteApiUser, c => c.DeleteApiUser(user.Username), Names.DeleteApiUser),
                    }
                }
            }).ToList();

            result.ForEach(c => c._links._actions.RemoveAll(d => d == null));
            var linkBuilderResult = new PageLinkBuilder(Url, Names.GetAllApiUsers, null, pageNo, pageSize, total);

            return Ok(new JobResult<object>()
            {
                SemasResult = new SemasResult<object>(true) { Data = result },
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

        /// <summary>
        /// Erstellt einen neuen API User  
        /// </summary>
        /// <remarks>
        /// ### REMARKS ###
        /// - Der API User wird zur Verwendung von geschützen API Operationen benötigt.
        /// - Der aktuelle APIUSer muss über die Berechtigung verfügen einen neuen API USer zu erstellen
        /// - NewApiUser gibt ein Ergebnis vom Typ JobResult zurück.
        /// #### Repsonse-Example ####
        /// <!-- 
        /// <code>
        ///     {
        ///          "semasResult": {
        ///          "data": null,
        ///          "success": true,
        ///          "message": "",
        ///          "errorcode": ""
        ///          },
        ///          "_links": {
        ///          "_actions": [{
        ///              "type": "POST",
        ///              "rel": "DeleteApiUser",
        ///              "href": "https://url:port/Admin/DeleteApiUser?username=username"
        ///          },
        ///          {
        ///              "type": "POST",
        ///              "rel": "RevokeAdminFromApiUser",
        ///              "href": "https://url:port/Admin/RevokeAdminFromApiUser?username=username"
        ///          }],
        ///          "_infos": {}
        ///          },
        ///          "_paging": null
        ///     }
        /// </code>
        /// -->
        /// </remarks>
        /// <response code="200">OK</response>
        /// <param name="username">username</param>
        /// <param name="passwort">password</param>
        /// <param name="asAdmin">default = false</param>
        /// <returns></returns>
        [HttpPost, Route(Names.Admin + "/" + Names.NewApiUser, Name = Names.NewApiUser)]
        public async Task<IHttpActionResult> NewApiUser(string username, string passwort, bool asAdmin = false)
        {
            IHandleJobs repo = new JobRepository(_username);
            var result = await repo.CreateApiUser(username, passwort, asAdmin);
            if (!result.Success)
                return Content(HttpStatusCode.BadRequest, result.Message);

            var apiResult = new JobResult<SemasResult>()
            {
                SemasResult = new SemasResult<SemasResult>(result.Success, result.Message),
                _links = new Link()
                {
                    _actions = new List<Action>()
                    {
                        CreateActionLink<AdminController>(Names.DeleteApiUser, c => c.DeleteApiUser(username), Names.DeleteApiUser),
                       !asAdmin ? CreateActionLink<AdminController>(Names.SetApiUserAsAdmin, c => c.SetApiUserAsAdmin(username), Names.SetApiUserAsAdmin): null,
                        asAdmin? CreateActionLink<AdminController>(Names.RevokeAdminFromApiUser, c => c.RevokeAdminFromApiUser(username), Names.RevokeAdminFromApiUser):null,
                    }
                }
            };
            apiResult._links._actions.RemoveAll(c => c == null);
            return Ok(apiResult);
        }


        /// <summary>
        /// Löscht einen APIUser
        /// </summary>
        /// <remarks>
        /// Erfordert die Admin-Rolle
        /// </remarks>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost, Route(Names.Admin + "/" + Names.DeleteApiUser, Name = Names.DeleteApiUser)]
        public async Task<IHttpActionResult> DeleteApiUser(string username)
        {
            IHandleJobs repo = new JobRepository(_username);
            var result = await repo.DeleteApiUser(username);
            if (!result.Success)
                return Content(HttpStatusCode.BadRequest, result.Message);

            var apiResult = new JobResult<SemasResult>()
            {
                SemasResult = new SemasResult<SemasResult>(result.Success, result.Message),
                _links = new Link()
                {
                    _actions = new List<Action>()
                    {
                        CreateActionLink<AdminController>(Names.NewApiUser, c => c.NewApiUser(username, "Change this text to the apipassword", false), Names.NewApiUser),
                    }
                }
            };
            return Ok(apiResult);
        }

        /// <summary>
        /// Berechtigt einen APIUser als Administrator
        /// </summary>
        /// <remarks>
        /// Erfordert die Admin-Rolle
        /// </remarks>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost, Route(Names.Admin + "/" + Names.SetApiUserAsAdmin, Name = Names.SetApiUserAsAdmin)]
        public async Task<IHttpActionResult> SetApiUserAsAdmin(string username)
        {
            IHandleJobs repo = new JobRepository(_username);
            var result = await repo.SetApiUserAsAdmin(username);
            if (!result.Success)
                return Content(HttpStatusCode.BadRequest, result.Message);

            var apiResult = new JobResult<SemasResult>()
            {
                SemasResult = new SemasResult<SemasResult>(result.Success, result.Message),
                _links = new Link()
                {
                    _actions = new List<Action>()
                    {
                        CreateActionLink<AdminController>(Names.DeleteApiUser, c => c.DeleteApiUser(username), Names.DeleteApiUser),
                        CreateActionLink<AdminController>(Names.RevokeAdminFromApiUser, c => c.RevokeAdminFromApiUser(username), Names.RevokeAdminFromApiUser)
                    }
                }
            };


            return Ok(apiResult);
        }

        /// <summary>
        /// Entzieht das Admin-Recht von einem APIUser
        /// </summary>
        /// <remarks>
        /// Erfordert die Admin-Rolle
        /// </remarks>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost, Route(Names.Admin + "/" + Names.RevokeAdminFromApiUser, Name = Names.RevokeAdminFromApiUser)]
        public async Task<IHttpActionResult> RevokeAdminFromApiUser(string username)
        {
            IHandleJobs repo = new JobRepository(_username);
            var result = await repo.RevokeAdminFromApiUser(username);
            if (!result.Success)
                return Content(HttpStatusCode.BadRequest, result.Message);


            var apiResult = new JobResult<SemasResult>()
            {
                SemasResult = new SemasResult<SemasResult>(result.Success, result.Message),
                _links = new Link()
                {
                    _actions = new List<Action>()
                    {
                        CreateActionLink<AdminController>(Names.DeleteApiUser, c => c.DeleteApiUser(username), Names.DeleteApiUser),
                        CreateActionLink<AdminController>(Names.SetApiUserAsAdmin, c => c.SetApiUserAsAdmin(username), Names.SetApiUserAsAdmin),
                    }
                }
            };
            return Ok(apiResult);
        }
    }
}
