using Microsoft.EntityFrameworkCore;

namespace DAtingApp.helpers
{
	public class PageList<T> :List<T>
	{
		public PageList(IEnumerable<T> items, int PageNumber, int count, int pageSize)
		{
			CurrentPage = PageNumber;
			TotalPages = (int)Math.Ceiling( count / (double) pageSize);
			PageSize = pageSize;
			TotalCount = count;
			AddRange(items);
		}

		public int CurrentPage { get; set; }

		public int TotalPages { get; set; }

		public int TotalCount { get; set; }

		public int PageSize { get; set; }

		public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, 
			int pageNumber, int pageSize)
		{
			int count = await source.CountAsync();

			var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

			return new PageList<T>(items, pageNumber, count, pageSize);
		} 

	}
}
