namespace DAtingApp.helpers
{
	public class PaginationHeader
	{
		public PaginationHeader(int currentPage, int pageSize, int totalItems, int totalPages)
		{
			CurrentPage = currentPage;
			PageSize = pageSize;
			TotalPages = totalPages;
			TotalItems = totalItems;
		}

		public int CurrentPage { get; set; }

		public int PageSize { get; set; }

		public int TotalPages { get; set; }	

		public int TotalItems { get; set; }

	}
}
