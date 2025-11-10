using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nerosoft.Euonia.Claims;

namespace Nerosoft.Starfish.Azfunc;

internal class JwtAuthenticationMiddleware : IFunctionsWorkerMiddleware
{
    public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpContext = context.GetHttpContext();

        ClaimsPrincipal principal = null;

        if (httpContext != null)
        {
            var token = httpContext.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", string.Empty);
            if (!string.IsNullOrEmpty(token))
            {
                var options = context.InstanceServices.GetService<IOptions<JwtAuthenticationOptions>>().Value;

                var key = Encoding.UTF8.GetBytes(options.SigningKey);

                var validation = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role,
                    ValidIssuers = options.Issuer,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                principal = new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
            }
        }

        principal ??= new ClaimsPrincipal();
        httpContext.User = principal;
        var user = new UserPrincipal(principal);

        if (FunctionContainer.Instance.TryGetFunction(context.FunctionDefinition.Name, out var method))
        {
            if (!method.HasAttribute<AllowAnonymousAttribute>())
            {
                var authorize = method.GetCustomAttribute<AuthorizeAttribute>();

                authorize ??= method.DeclaringType.GetCustomAttribute<AuthorizeAttribute>();

                if (authorize != null)
                {
                    if (!user.IsAuthenticated)
                    {
                        httpContext.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }

                    if (!string.IsNullOrWhiteSpace(authorize.Roles))
                    {
                        var roles = authorize.Roles.Split(",");
                        if (!user.IsInRoles(roles))
                        {
                            httpContext.Response.StatusCode = 403;
                            return Task.CompletedTask;
                        }
                    }
                }
            }
        }

        context.Items.Add("User", user);

        return next(context);
    }
}