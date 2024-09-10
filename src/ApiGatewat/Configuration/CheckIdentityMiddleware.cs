using Microsoft.AspNetCore.Routing;
using Ocelot.Configuration;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiGatewat.Configuration;

public class CheckIdentityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CheckIdentityMiddleware> _logger;

    public CheckIdentityMiddleware(
        RequestDelegate next, ILogger<CheckIdentityMiddleware> logger, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public string GetRoutesByUpstreamPathTemplate(string upstreamPathTemplate)
    {
        var configurationSection = new ConfigurationBuilder()
            .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
            .Build().GetSection("Routes");

        
        var upstream = configurationSection.GetChildren()
            .Where(r => r["UpstreamPathTemplate"].Contains(upstreamPathTemplate));

        var downstreamPath = upstream.Select(r => r.GetSection("DownstreamPathTemplate")).FirstOrDefault().Value;
        var downstream = upstream.Select(r => r.GetSection("DownstreamHostAndPorts").GetChildren()
            .Select(h => new { Host = h.GetSection("Host").Value, Port = h.GetSection("Port").Value })).ToList();

        var route = downstream.Select(r => new 
        {
            Host = r.Select(x => x.Host).FirstOrDefault(),
            Port = r.Select(x => x.Port).FirstOrDefault(),
        }).FirstOrDefault();

        var routes = $"{route.Host}:{route.Port}{downstreamPath}";
        return routes;
    }


    public async Task Invoke(HttpContext context)
    {
        await _next(context).ConfigureAwait(true);
        _logger.LogInformation("Processing response...");

        var routeTest = context.Request.Path;
        

        //if (context.Response.StatusCode == 401 && !context.User.Identity.IsAuthenticated)
        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {

            var returnUrl = context.Request.Path;
            //var route = GetRoutesByUpstreamPathTemplate(routeTest);
            

            var httpClient = _httpClientFactory.CreateClient();
            //var redirectUrl = $"http://localhost:3000/auth/signin?returnUrl={Uri.EscapeDataString(path)}";
            //var redirectUrl = $"http://localhost:3000/auth/signin?returnUrl={route}";
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
