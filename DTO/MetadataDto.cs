namespace backend2.DTO {
    public class MetadataDto {


        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }

        public MetadataDto( ) {
        }

        public MetadataDto(int totalCount, int pageSize, int currentPage, int totalPages, bool hasNext, bool hasPrevious) {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            HasNext = hasNext;
            HasPrevious = hasPrevious;
        }
    }
}
