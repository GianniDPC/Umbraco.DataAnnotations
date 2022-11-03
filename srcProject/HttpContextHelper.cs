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
        public static HttpContext Current => HttpContextAccessor.HttpContext;

        private static readonly HttpContextAccessor HttpContextAccessor = new HttpContextAccessor();
#else
        public static HttpContext Current => HttpContext.Current;
#endif
    }
}
