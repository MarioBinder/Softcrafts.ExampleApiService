using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Softcrafts.JobService.API.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (!apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                var basicAuthDict = new Dictionary<string, IEnumerable<string>>();
                basicAuthDict.Add("basic", new List<string>());
                operation.security = new IDictionary<string, IEnumerable<string>>[] { basicAuthDict };
            }

            operation.description = Formatted(operation.description);
            operation.summary = Formatted(operation.summary);
        }

        private string Formatted(string text)
        {
            if (text == null) return null;
            string resultString = Regex.Replace(text, @"(^[ \t]+)(?![^<]*>|[^>]*<\/)", "", RegexOptions.Multiline);
            resultString = Regex.Replace(resultString, @"<code[^>]*>", "<pre>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
            resultString = Regex.Replace(resultString, @"</code[^>]*>", "</pre>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
            resultString = Regex.Replace(resultString, @"<!--", "", RegexOptions.Multiline);
            resultString = Regex.Replace(resultString, @"-->", "", RegexOptions.Multiline);
            return resultString;
            try
            {
                string pattern = @"<pre\b[^>]*>(.*?)</pre>";

                foreach (Match match in Regex.Matches(resultString, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline))
                {
                    var formattedPreBlock = FormatPreBlock(match.Value);
                    resultString = resultString.Replace(match.Value, formattedPreBlock);
                }
                return resultString;
            }
            catch
            {
                // Something went wrong so just return the original resultString
                return resultString;
            }
        }

        private string FormatPreBlock(string preBlock)
        {
            // Split the <pre> block into multiple lines
            var linesArray = preBlock.Split('\n');
            if (linesArray.Length < 2)
            {
                return preBlock;
            }
            else
            {
                // Get the 1st line after the <pre>
                string line = linesArray[1];
                int lineLength = line.Length;
                string formattedLine = line.TrimStart(' ', '\t');
                int paddingLength = lineLength - formattedLine.Length;

                // Remove the padding from all of the lines in the <pre> block
                for (int i = 1; i < linesArray.Length - 1; i++)
                {
                    linesArray[i] = linesArray[i].Substring(paddingLength);
                }

                var formattedPreBlock = string.Join("", linesArray);
                return formattedPreBlock;
            }

        }
    }
}
