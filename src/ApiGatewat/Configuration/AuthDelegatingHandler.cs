using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Ocelot.RequestId;
using System.Net;
using System.Text;

namespace ApiGatewat.Configuration;

public class AuthDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    //private readonly HttpContext context;
    public AuthDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

    }
    //private readonly IHttpContextAccessor _httpContextAccessor;
    //public AuthDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    //{ 
    //    //_next = next;
    //    _httpContextAccessor = httpContextAccessor;
    //}


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        Console.Clear();

        if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Bearer")
        {
            var token2 = request.Headers.Authorization.Parameter;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token2);
        }

        //if (request.Headers.TryGetValues("Authorization", out var token))
        //{
        //    var tokenValues = token.FirstOrDefault();
        //    Console.WriteLine($"{token}");
        //    Console.WriteLine($"{tokenValues}");
        //}

        var response = await base.SendAsync(request, ct);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var testAuth = httpContext.User.Identity.IsAuthenticated;
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                var redirectRequest = new HttpRequestMessage(HttpMethod.Get, request.RequestUri);
                var returnUrl = httpContext.Request.Path;
                var redirectUrl = $"http://localhost:3000/auth/signin?returnUrl={returnUrl}";
            }

            var url = $"http://localhost:3000/auth/signin?returnUrl={httpContext.Request.Path}";
            var redirectResponse = new HttpResponseMessage(HttpStatusCode.Redirect)
            {
                Headers = { Location = new Uri($"{url}") }
            };
            return redirectResponse;
        }
        var content = response.Content.ReadAsStringAsync();
        var jsonResponse = new HttpResponseMessage(response.StatusCode)
        {
            Content = new StringContent(await response.Content.ReadAsStringAsync(), Encoding.UTF8, "application/json")
        };
        return response;
    }

    public async Task Redirect(HttpContext context)
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            var returnUrl = context.Request.Path;
            var redirectUrl = $"http://localhost:3000/auth/signin?returnUrl={returnUrl}";
            context.Response.Redirect(returnUrl);
            return;
        }
    }
}

