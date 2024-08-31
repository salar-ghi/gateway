using Microsoft.AspNetCore.Routing;
using Ocelot.Configuration;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiGatewat.Configuration;

public class CheckIdentityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CheckIdentityMiddleware> _logger;

    public CheckIdentityMiddleware(
        RequestDelegate next,
        ILogger<CheckIdentityMiddleware> logger,
        IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }


    public async Task Invoke(HttpContext context)
    {
        await _next(context).ConfigureAwait(true);
        _logger.LogInformation("Processing response...");

        var routeTest = context.Request.Path;
        var routes = context.Request.PathBase;
        var routeTest2 = context.Request.HttpContext.GetRouteValue(routeTest);
        var routeTest3 = context.Request.HttpContext.GetRouteValue(routes.ToString());

        //if (context.Response.StatusCode == 401 && !context.User.Identity.IsAuthenticated)
        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            //if (context.Items.TryGetValue("DownstreamRoute", out var downstreamRoute))
            //{
            //    var route = downstreamRoute as DownstreamRoute;
            //    // Get the downstream host and port
            //    var downstreamHost = route.DownstreamAddresses.FirstOrDefault().Host; // Assuming the first host
            //    var downstreamPort = route.DownstreamAddresses.FirstOrDefault().Port; // Get the port

            //    // Log the downstream host and port
            //    Console.WriteLine($"Downstream Host: {downstreamHost}, Downstream Port: {downstreamPort}");
            //}

            var returnUrl = context.Request.Path;

            var httpClient = _httpClientFactory.CreateClient();
            //var redirectUrl = $"http://localhost:3000/auth/signin?returnUrl={Uri.EscapeDataString(path)}";
            var redirectUrl = $"http://localhost:3000/auth/signin?returnUrl={returnUrl}";
            context.Response.Redirect(redirectUrl);
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
