using TurboProject.Models.Entities.Common;

namespace TurboProject.Models.Entities
{
    public class Currency
    {
        public byte Id { get; set; }
        public string CurrencyCode { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
    }   
}
