using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Text;
using OCC.Logging;
using Softcrafts.Jobs.Entities;
using Softcrafts.Jobs.Entities.Base;
using Softcrafts.Jobs.Business.Data;

namespace Softcrafts.Jobs.Business
{
    public partial class JobRepository : IHandleJobs
    {
        public string Username { get; }
        readonly ApplicationLogger _logger = new ApplicationLogger("Softcrafts.JobService");

        public JobRepository(string username)
        {
            Username = username;
        }
        
        public async Task<SemasResult<List<Job>>> GetAllJobs(Func<Job, bool> predicate)
        {
            _logger.Info($"ApiUser {Username} retrieved GetAllJobs");
            using (var ctx = new JobContext())
            {
                var data = await new AsyncEnumerableQuery<Job>(ctx.Jobs.Where(predicate)).ToListAsync();
                var result = new SemasResult<List<Job>>(true, data);
                return result;
            }
        }

        public async Task<SemasResult<List<Job>>> GetNextOpenJobs()
        {
            _logger.Info($"ApiUser {Username} retrieved GetNextOpenJobs");
            using (var ctx = new JobContext())
            {
                var data = await new AsyncEnumerableQuery<Job>(ctx.Jobs.Where(c => !c.InWork && !c.Done && !c.IsCanceled)).ToListAsync();
                var result = new SemasResult<List<Job>>(true, data);
                return result;
            }
        }

        public async Task<SemasResult<List<Job>>> GetCanceledJobs()
        {
            _logger.Info($"ApiUser {Username} retrieved GetCanceledJobs");
            using (var ctx = new JobContext())
            {
                var data = await new AsyncEnumerableQuery<Job>(ctx.Jobs.Where(c => c.IsCanceled)).ToListAsync();
                var result = new SemasResult<List<Job>>(true, data);
                return result;
            }
        }

        public async Task<SemasResult<Job>> SaveJob(Job job)
        {
            try
            {
                using (var ctx = new JobContext())
                {
                    var savedJob = ctx.Jobs.FirstOrDefault(c => c.UniqueId == job.UniqueId);
                    if (savedJob != null)
                    {
                        job.UpdatedDate = DateTime.Now;
                        job.UpdatedUser = Username;
                        ctx.Entry(savedJob).CurrentValues.SetValues(job);
                    }
                    else
                    {
                        job.UniqueId = Guid.NewGuid();
                        job.CreatedUser = Username;
                        job.CreatedDate = DateTime.Now;
                        ctx.Jobs.Add(job);
                    }
                    _logger.Info($"ApiUser {Username} saved a Job ({job.JobName}, UniqueId: {job.UniqueId}");
                    var result = await ctx.SaveChangesAsync();
                    return new SemasResult<Job>(result > 0) { Data = job };
                }
            }
            catch (DbEntityValidationException e)
            {
                var errIds = new List<Guid>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        var errId = Guid.NewGuid();
                        errIds.Add(errId);
                        _logger.Error($"SaveJob-{errId} throws an DbEntityValidationException: (ValidationError: {ve.PropertyName}, {ve.ErrorMessage})  - ApiUser: {Username}, Jobname: {job.JobName}, UniqueId: {job.UniqueId}");
                    }
                }
                return new SemasResult<Job>(false, $"{errIds.Select(c => c + ",")}");
            }
            catch (Exception e)
            {
                var errId = Guid.NewGuid();
                _logger.Error($"SaveJob-{errId} throws an Exception: ({e.Message}, {e.InnerException?.Message})  - ApiUser: {Username}, Jobname: {job.JobName}, UniqueId: {job.UniqueId}");
                return new SemasResult<Job>(false, $"{errId}");
            }
        }

        private static string Random(int length = 8, string chars = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
        {
            var randomString = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < length; i++)
                randomString.Append(chars[random.Next(chars.Length)]);

            return randomString.ToString();
        }
    }
}
