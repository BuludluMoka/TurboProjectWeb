using TurboProject.Models.Entities.Common;

namespace TurboProject.Models.Entities
{
    public class CarImageFile : BaseEntity
    {
        public string Image { get; set; }
        public long AnnouncementId { get; set; }
        public Announcement Announcement { get; set; }

    }
}
