using TurboProject.Models.Entities.Common;
using TurboProject.Models.Entities.Identity;

namespace TurboProject.Models.Entities
{
    public class Announcement : BaseEntity
    {
        public decimal Price { get; set; }
        public int Year { get; set; }
        public long ModelId { get; set; }
        public Model Model { get; set; }
        public byte BanTypeId { get; set; }
        public BanType BanType { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public byte CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public ICollection<CarImageFile> CarImageFiles { get; set; }

    }
}
