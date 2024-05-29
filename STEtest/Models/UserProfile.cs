using Microsoft.AspNetCore.Identity;

namespace STEtest.Models
{
    public class UserProfile : IdentityUser
    { 
      public string UserType { get; set; }
    }
}
