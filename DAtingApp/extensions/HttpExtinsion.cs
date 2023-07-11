using DAtingApp.helpers;
using System.Text.Json;

namespace DAtingApp.extensions
{
	public static class HttpExtinsion
	{
		public static void AddPaginationHeader(this HttpResponse response,PaginationHeader header)
		{
			var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

			response.Headers.Add("Pagination",JsonSerializer.Serialize(header,jsonOptions));

			response.Headers.Add("Access-Control-Expose-Headers", "Pagination");



		}
	}
}
