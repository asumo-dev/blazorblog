namespace BlazorBlog.Tests.Common
{
    public static class StringExtensions
    {
        public static QueryString AsQueryString(this string str)
        {
            return new(str);
        }
    }
}
