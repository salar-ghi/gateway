namespace ApiGatewat.Configuration;

public class CustomDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Custom logic before sending the request
        Console.WriteLine("Request URI: " + request.RequestUri);

        var response = await base.SendAsync(request, cancellationToken);

        // Custom logic after receiving the response
        return response;
    }
}