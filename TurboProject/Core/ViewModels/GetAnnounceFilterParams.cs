namespace TurboProject.Core.ViewModels
{
    public class GetAnnounceFilterParams
    {
        public int? StartYear { get; set; } 
        public int? EndYear { get; set; } 
        public decimal? StartPrice { get; set; } 
        public decimal? EndPrice { get; set; }
        public int? CurrId { get; set; }
        public int? BanId { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int PageSize { get; set; } = 8;
        public int PageIndex { get; set; } = 1;
    }
}
