using Softcrafts.Jobs.Entities.Base;

namespace Softcrafts.JobService.API.Models
{
    /// <summary>
    /// JobResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JobResult<T>
    {
        /// <summary>
        /// SemasResult
        /// </summary>
        public SemasResult<T> SemasResult { get; set; }
        /// <summary>
        /// </summary>
        public Link _links { get; set; } = new Link();
        /// <summary>
        /// </summary>
        public object _paging { get; set; }
    }


}
