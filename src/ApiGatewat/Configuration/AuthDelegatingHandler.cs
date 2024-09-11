namespace ApiGatewat.Configuration;

public class AuthDelegatingHandler : DelegatingHandler
{
    
    //private readonly IHttpContextAccessor _httpContextAccessor;
    //public AuthDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    //{ 
    //    //_next = next;
    //    _httpContextAccessor = httpContextAccessor;
    //}


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        Console.Clear();
        Console.WriteLine($"+==========================> this is Requested Host URl =>{request.RequestUri}");
        var response = await base.SendAsync(request, ct);
        //System.Diagnostics.Debug.WriteLine($"Response: {response.StatusCode}");
        //Console.WriteLine($"status Code ========+++++++=======> {response.StatusCode}");
        //if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //{
        //    Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<============================hello every body 2============================>>>>>>>>>>>>>>>>");
        //    Console.WriteLine($"that's ok man.............................. bro :))))))");

        //    var redirectUrl = $"http://localhost:3000/auth/signin?returnUrl={request.RequestUri}";
        //    //var redirectUrl = "https://localhost:5010/api/Auth/Index";
        //    //var redirect = await RedirectRequest(redirectUrl, ct);

        //    using (var client = new HttpClient())
        //    {
        //        // new request
        //        var redirectRequest = new HttpRequestMessage(HttpMethod.Get, redirectUrl);
        //        return await client.SendAsync(redirectRequest, ct);
        //    }
        //    //context.Response.Redirect("https://localhost:5010/api/Auth/Index");
        //    //return;
        //}

        return response;
    }
}

