using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurboProject.Models.Entities
{
    [Keyless]
    public class FilterAnnouncements
    {
        public long? Id { get; set; }
        public string? MakeName { get; set; }
        public string? ModelName { get; set; }
        public decimal? Price { get; set; }
        public int? Year { get; set; }
        public string? Ban { get; set; }
        public string? Currency { get; set; }
        public string? Image { get; set; }
    }
}
