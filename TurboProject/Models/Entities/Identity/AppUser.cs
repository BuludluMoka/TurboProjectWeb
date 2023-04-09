using Microsoft.AspNetCore.Identity;

namespace TurboProject.Models.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public ICollection<Announcement> Announcements { get; set; }

    }
}
