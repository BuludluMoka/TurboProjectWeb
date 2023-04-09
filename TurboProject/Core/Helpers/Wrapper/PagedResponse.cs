using Azure;

namespace TurboProject.Core.Helpers.Wrapper
{
    public class PagedResponse<T> 
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public T Data { get; set; }


        public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
        {

            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalPages = Convert.ToInt32(Math.Ceiling((double)totalRecords / (double)pageSize));
            this.TotalRecords = totalRecords;
            this.Data = data;
        }
    }
}
