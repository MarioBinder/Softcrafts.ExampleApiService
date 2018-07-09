using System.IO;

namespace Softcrafts.JobService.API.Models
{
    /// <summary>
    /// Represents a model that contain information and data about received HttpRequest.
    /// </summary>
    public class HttpRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PathBase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Uri => string.IsNullOrWhiteSpace(QueryString)
            ? Scheme + "://" + Host + PathBase + Path
            : Scheme + "://" + Host + PathBase + Path + QueryString;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ConvertToString(Stream stream)
        {
            try
            {
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    var serializedStream = reader.ReadToEnd();

                    return serializedStream;
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }
    }
}
