namespace ApiGatewat.Configuration;

public class AuthDelegatingHandler :  DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        System.Diagnostics.Debug.WriteLine($"Request: {request.Method}{request.RequestUri}");
        var response = await base.SendAsync(request, ct);
        System.Diagnostics.Debug.WriteLine($"Response: {response.StatusCode}");
        Console.WriteLine($"status Code ========+++++++=======> {response.StatusCode}");
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"that's ok man.............................. bro :))))))");
            var redirectUrl = "https://localhost:5010/api/Auth/Index";
            var redirect = await RedirectRequest(redirectUrl, ct);
            return redirect;

            //context.Response.Redirect("https://localhost:5010/api/Auth/Index");
            //return;
        }
        
        return response;
    }



    private async Task<HttpResponseMessage> RedirectRequest(string url, CancellationToken ct)
    {
        using (var client = new HttpClient())
        {
            // new request
            var redirectRequest = new HttpRequestMessage(HttpMethod.Get, url);
            return await client.SendAsync(redirectRequest, ct);
        }
    }
}
