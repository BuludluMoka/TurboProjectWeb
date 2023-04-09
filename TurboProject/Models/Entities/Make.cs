using TurboProject.Models.Entities.Common;

namespace TurboProject.Models.Entities
{
    public class Make : BaseEntity
    {
        public string MakeName { get; set; }
        public ICollection<Model> Models { get; set; }
    }
}
