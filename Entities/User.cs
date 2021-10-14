using Microsoft.AspNetCore.Identity;

namespace DisneyAPI.Entities
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; }
    }
}
