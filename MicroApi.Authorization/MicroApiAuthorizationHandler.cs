using MicroApi.Core.HandleResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MicroApi.Authorization
{
    public class MicroApiAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, IHandleResponse>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, IHandleResponse resource)
        {
            if (context.User.HasClaim(m=>m.Type == ClaimTypes.Name))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }

    public static class MicroApiAuthorizationExtension
    {
        public static IServiceCollection AddMicroApiAuthorization(this IServiceCollection services) =>
            services.AddSingleton<IAuthorizationHandler, MicroApiAuthorizationHandler>();
    }
}
