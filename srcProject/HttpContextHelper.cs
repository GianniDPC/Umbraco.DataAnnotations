using Microsoft.AspNetCore.Http;


namespace Umbraco.DataAnnotations
{
    internal static class HttpContextHelper
    {
        public static HttpContext Current => HttpContextAccessor.HttpContext;
        private static readonly HttpContextAccessor HttpContextAccessor = new HttpContextAccessor();
    }
}
