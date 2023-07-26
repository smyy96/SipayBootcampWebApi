using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class FakeAuthorizationAttribute : TypeFilterAttribute
{
    public FakeAuthorizationAttribute() : base(typeof(FakeAuthorizationFilter))
    {
    }
}

public class FakeAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // fake bir kullanıcı adı ve şifresini kontrolü
        string username = context.HttpContext.Request.Headers["X-Fake-Username"];
        string password = context.HttpContext.Request.Headers["X-Fake-Password"];

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (username == "fakeuser" && password == "fakepassword")
        {
            context.Result = new OkObjectResult("Fake kullanıcı girişi başarılı!");
        }
        else
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
