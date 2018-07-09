using System;
using System.Web.Http;

namespace Softcrafts.jobservice.API.Controllers
{
    /// <summary>
    /// JobDetailController
    /// </summary>
    public class JobDetailController : JobBaseController
    {
        private readonly string _username;

        /// <summary>
        /// JobDetailController
        /// </summary>
        public JobDetailController()
        {
            _username = GetCurrentApiUser();
        }


        /// <summary>
        /// Ermittelt eine Job anhand der UniqueId
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        [HttpGet, Route("JobDetail/JobDetails/{uniqueId}", Name = Names.JobDetails)]
        public IHttpActionResult JobDetails(Guid uniqueId)
        {
            return Ok(uniqueId);
        }

        /// <summary>
        /// Setzt einen Job auf done
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        [HttpPut, Route("JobDetail/SetzeJobDone/{uniqueId}", Name = Names.SetzeJobDone)]
        public IHttpActionResult SetzeJobDone(Guid uniqueId)
        {
            return Ok(uniqueId);
        }

        /// <summary>
        /// Setzt einen Job auf inWork
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        [HttpPut, Route("JobDetail/SetzeJobInWork/{uniqueId}", Name = Names.SetzeJobInWork)]
        public IHttpActionResult SetzeJobInWork(Guid uniqueId)
        {
            return Ok(uniqueId);
        }

        /// <summary>
        /// Setzt einen Job auf IcCanceled
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        [HttpPut, Route("JobDetail/SetzeJobIsCanceled/{uniqueId}", Name = Names.SetzeJobIsCanceled)]
        public IHttpActionResult SetzeJobIsCanceled(Guid uniqueId)
        {
            return Ok(uniqueId);
        }

        /// <summary>
        /// Setzt das canceled-Flag eines Jobs zurück
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        [HttpPut, Route("JobDetail/LoescheJobIsCanceledFlag/{uniqueId}", Name = Names.LoescheJobIsCanceledFlag)]
        public IHttpActionResult LoescheJobIsCanceledFlag(Guid uniqueId)
        {
            return Ok(uniqueId);
        }

        /// <summary>
        /// Löscht einen Job
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        [HttpDelete, Route("JobDetail/LoescheJob/{uniqueId}", Name = Names.LoescheJob)]
        public IHttpActionResult LoescheJob(Guid uniqueId)
        {
            return Ok(uniqueId);
        }
    }
}
