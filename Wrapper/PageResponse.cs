namespace ShopAPI.Wrapper
{
    public class PageResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }


        public PageResponse(T data, int pageNumber, int pageSize,int TotalRecords)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = TotalRecords;
            this.TotalPages = TotalRecords / pageSize + 1;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
