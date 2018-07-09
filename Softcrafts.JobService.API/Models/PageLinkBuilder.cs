using System;
using System.Web.Http.Routing;

namespace Softcrafts.JobService.API.Models
{
    /// <summary>
    /// </summary>
    public class PageLinkBuilder
    {
        /// <summary>
        /// </summary>
        public Uri FirstPage { get; private set; }
        /// <summary>
        /// </summary>
        public Uri LastPage { get; private set; }
        /// <summary>
        /// </summary>
        public Uri NextPage { get; private set; }
        /// <summary>
        /// </summary>
        public Uri PreviousPage { get; private set; }

        /// <summary>
        /// </summary>
        public int TotalPageCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecordCount"></param>
        public PageLinkBuilder(UrlHelper urlHelper, string routeName, object routeValues, int pageNo, int pageSize, long totalRecordCount)
        {
            // Determine total number of pages
            var pageCount = TotalPageCount = totalRecordCount > 0 ?
                (int)Math.Ceiling(totalRecordCount / (double)pageSize) : 0;
            
            FirstPage = new Uri(urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
            {
                {"pageNo", 1},
                {"pageSize", pageSize}
            }));
            LastPage = new Uri(urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
            {
                {"pageNo", pageCount},
                {"pageSize", pageSize}
            }));
            if (pageNo > 1)
            {
                PreviousPage = new Uri(urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
                {
                    {"pageNo", pageNo - 1},
                    {"pageSize", pageSize}
                }));
            }
            if (pageNo < pageCount)
            {
                NextPage = new Uri(urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
                {
                    {"pageNo", pageNo + 1},
                    {"pageSize", pageSize}
                }));
            }
        }
    }
}
