using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiGatewat.Configuration;

public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
{
    private readonly IAuthorizationService _authorizationService;
    public CustomAuthorizeFilter(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }


    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var result = await _authorizationService.AuthorizeAsync(context.HttpContext.User, null, "MultiAuthPolicy");
        if (!result.Succeeded)
        {
            context.Result = new ForbidResult();
        }
        throw new NotImplementedException();
    }
}
