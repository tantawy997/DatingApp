using DatingApp.Entites;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}


		public virtual DbSet<AppUser> Users { get; set; } 

	}
}
