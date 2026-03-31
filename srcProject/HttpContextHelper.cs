#if NET || NETCOREAPP
using Microsoft.AspNetCore.Http;
#else
using System.Web;
#endif


namespace Umbraco.DataAnnotations
{
    internal static class HttpContextHelper
    {

#if NET || NETCOREAPP
        public static HttpContext Current => _httpContextAccessor?.HttpContext;

        private static IHttpContextAccessor _httpContextAccessor;

        internal static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }
#else
        public static HttpContext Current => HttpContext.Current;
#endif
    }
}
