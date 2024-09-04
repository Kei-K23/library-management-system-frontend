// Services/HttpContextService.cs
namespace LibraryManagementSystemApp.Services
{
    public class HttpContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCookie(string cookieName)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[cookieName];
        }
    }

}
