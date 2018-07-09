using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Text;
using System.Web.Http;
using System.Net.Http.Headers;
using Softcrafts.JobService.API.Security;
using Ploeh.Hyprlinkr;
using Action = Softcrafts.Jobs.Entities.Base.Action;

namespace Softcrafts.jobservice.API.Controllers
{
    /// <inheritdoc />
    [BasicAuth]
    public class JobBaseController : ApiController
    {
        internal string GetCurrentApiUser()
        {
            var request = HttpContext.Current.Request;
            if (!request.IsAuthenticated)
                return "anonymous";

            var authHeader = request.Headers["Authorization"];
            var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var user = encoding.GetString(Convert.FromBase64String(authHeaderVal.Parameter));
            return user?.Split(':')[0];
        }

        internal string GetRootUrl()
        {
            return Request?.RequestUri?.GetLeftPart(UriPartial.Scheme) + Request?.RequestUri?.Host + ":" +
                   Request?.RequestUri?.Port;
        }

        private string GetVerb<T>(string methodName)
        {
            var methods = typeof(T).GetMethods();
            string verb = string.Empty;

            var method = methods.FirstOrDefault(c => c.Name == methodName);
            if (method == null)
                return default(string);

            if (method.IsConstructor || !method.IsPublic || method.DeclaringType != typeof(T)) return verb;

            if (Attribute.IsDefined(method, typeof(System.Web.Http.HttpGetAttribute)))
            {
                verb = "GET";
            }

            if (Attribute.IsDefined(method, typeof(System.Web.Http.HttpPostAttribute)))
            {
                verb = "POST";
            }

            if (Attribute.IsDefined(method, typeof(System.Web.Http.HttpDeleteAttribute)))
            {
                verb = "DELETE";
            }

            if (Attribute.IsDefined(method, typeof(System.Web.Http.HttpPutAttribute)))
            {
                verb = "PUT";
            }

            if (Attribute.IsDefined(method, typeof(System.Web.Http.HttpPatchAttribute)))
            {
                verb = "PATCH";
            }

            return verb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Action CreateActionLink<T>(string rel, Expression<Action<T>> expression, string method) where T : ApiController
        {
            try
            {
                return new Action
                {
                    rel = rel,
                    href = Url.GetLink<T>(expression).ToString(),
                    type = GetVerb<T>(method)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
