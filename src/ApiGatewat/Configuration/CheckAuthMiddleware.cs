namespace ApiGatewat.Configuration;

public class CheckAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _identityServiceUrl;
    private readonly string[] _protectedRoutes;

    public CheckAuthMiddleware(RequestDelegate next, 
        string identityServiceUrl, 
        string[] protectedRoutes)
    {
        _next = next;
        _identityServiceUrl = identityServiceUrl;
        _protectedRoutes = protectedRoutes;

    }

    public async Task Invoke(HttpContext context)
    {
        // check if the current request path matches any of the protected routes
        if(_protectedRoutes.Contains(context.Request.Path.Value))
        {
            // Check if the User is authenticated
            if (!context.User.Identity.IsAuthenticated)
            {
                // Redirect to the Identity service for authentication
                context.Response.Redirect(_identityServiceUrl);
                return;
            }
        }
        // if the request is not for a protected route or the user is authenticated,
        // continue to the next middleware
        await _next(context);
    }

}
