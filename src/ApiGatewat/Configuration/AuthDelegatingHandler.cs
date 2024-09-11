namespace ApiGatewat.Configuration;

public class AuthDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        //_context = context;
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
        Console.Clear();
        //Console.WriteLine($"+==========================> this is Requested Host URl =>{request.RequestUri}");
        var response = await base.SendAsync(request, ct);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            //    var redirectUrl = $"http://localhost:3000/auth/signin?returnUrl={request.RequestUri}";
            var redirectUrl = "https://localhost:5010/api/Auth/Index";
            
            var context = _httpContextAccessor.HttpContext;

            using (var client = new HttpClient())
            {
                var redirectRequest = new HttpRequestMessage(HttpMethod.Get, request.RequestUri);
                context.Response.Redirect(redirectUrl);
            }
        }
        return response;
    }
}

