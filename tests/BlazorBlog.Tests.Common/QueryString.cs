using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace BlazorBlog.Tests.Common
{
    public class QueryString
    {
        private readonly NameValueCollection _params;

        public QueryString(string str)
        {
            _params = HttpUtility.ParseQueryString(str);
        }

        public bool Contains(NameValueCollection pairs)
        {
            return _params
                .AllKeys
                .All(key => _params[key] == pairs[key]);
        }
    }
}
