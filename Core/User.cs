using Microsoft.AspNetCore.Identity;

namespace Core
{
    public class User : IdentityUser
    {
    public string? UserNickName { get; set; }
    }
}
