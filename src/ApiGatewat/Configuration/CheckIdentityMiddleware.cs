namespace ApiGatewat.Configuration;

public class CheckIdentityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CheckIdentityMiddleware> _logger;

    public CheckIdentityMiddleware(RequestDelegate next, ILogger<CheckIdentityMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task Invoke(HttpContext context)
    {
        await _next(context).ConfigureAwait(true);
        _logger.LogInformation("Processing response...");

        //if (context.Response.StatusCode == 401 && !context.User.Identity.IsAuthenticated)
        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            //
            context.Response.Redirect("https://localhost:5010/api/Auth/Index");
            return;
        }

    }

    //public async Task Invoke(HttpContext context)
    //{
    //    await _next(context).ConfigureAwait(false);
    //    _logger.LogInformation("Processing response...");


    //    var statusCode = context.Response.StatusCode;
    //    //var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    //    if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
    //    {
    //        Console.WriteLine($"fuck you boy .........");
    //    }
    //    //else
    //    //{
    //    //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //    //    await context.Response.WriteAsync("Unauthorized");
    //    //}
    //}

    private bool ValidateToken(string token)
    {
        return true;
    }


}
