using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

public class UserTypeHandler : AuthorizationHandler<UserTypeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserTypeRequirement requirement)
    {
        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            var userType = context.User.Claims.FirstOrDefault(c => c.Type == "UserType")?.Value;
            if (userType != null && userType == requirement.UserType)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
