namespace ApiGatewat.Configuration;

public class CheckIdentityMiddleware
{
    private readonly RequestDelegate _next;

    public CheckIdentityMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    //public async Task Invoke(HttpContext context)
    //{
    //    //if (context.Response.StatusCode == 401 && !context.User.Identity.IsAuthenticated)
    //    if (context.Response.StatusCode == 401)
    //    {
    //        //
    //        context.Response.Redirect("https://localhost:5010/api/Auth/Index");
    //        return;
    //    }

    //    await _next(context);
    //}

    public async Task Invoke(HttpContext context)
    {
        var statusCode = context.Response.StatusCode;
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (!string.IsNullOrEmpty(token) && ValidateToken(token))
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }
    }

    private bool ValidateToken(string token)
    {
        return true;
    }


}
