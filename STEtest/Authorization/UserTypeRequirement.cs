using Microsoft.AspNetCore.Authorization;

public class UserTypeRequirement : IAuthorizationRequirement
{
    public string UserType { get; }

    public UserTypeRequirement(string userType)
    {
        UserType = userType;
    }
}
