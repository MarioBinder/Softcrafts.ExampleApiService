using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softcrafts.Jobs.Entities;
using Softcrafts.Jobs.Entities.Base;

namespace Softcrafts.Jobs.Business
{
    public interface IHandleJobs
    {
        SemasResult HeartBeat(bool inklDatabase);

        SemasResult IsApiUserAllowed(string username, string passwort);

        Task<SemasResult<List<APIUser>>> GetAllApiUsers(Func<APIUser, bool> predicate);
        Task<SemasResult> CreateApiUser(string username, string passwort, bool asAdmin = false);
        Task<SemasResult> SetApiUserAsAdmin(string username);
        Task<SemasResult> RevokeAdminFromApiUser(string username);
        Task<SemasResult> DeleteApiUser(string username);


        Task<SemasResult<List<Job>>> GetAllJobs(Func<Job, bool> predicate);
        Task<SemasResult<List<Job>>> GetNextOpenJobs();
        Task<SemasResult<List<Job>>> GetCanceledJobs();
        Task<SemasResult<Job>> SaveJob(Job job);
    }
}
