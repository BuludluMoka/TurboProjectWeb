using TurboProject.Models.Entities.Common;

namespace TurboProject.Models.Entities
{
    public class Model : BaseEntity
    {
        public string ModelName { get; set; }
        public long MakeId { get; set; }
        public Make Make { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
    }
}
