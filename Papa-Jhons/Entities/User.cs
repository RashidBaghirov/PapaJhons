using Microsoft.AspNetCore.Identity;

namespace Papa_Jhons.Entities
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
    }
}
