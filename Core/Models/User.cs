using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models
{
    public class User : IdentityUser
    {
    public string? UserNickName { get; set; }
    }
}
