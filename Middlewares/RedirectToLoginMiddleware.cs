namespace LibraryManagementSystemApp.Middlewares
{
    public class RedirectToLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectToLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated && !context.Request.Path.StartsWithSegments("/login"))
            {
                context.Response.Redirect("/login");
            }
            else
            {
                await _next(context);
            }
        }
    }

}
