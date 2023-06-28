using DatingApp.Entites;
using DAtingApp.Entites;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingApp.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}


		public virtual DbSet<AppUser> Users { get; set; }
		public virtual DbSet<Photo> Photos { get; set; }
		public virtual DbSet<UserLike> Likes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserLike>()
				.HasKey(k => new {k.SourceUserId, k.TargetUserId});

			modelBuilder.Entity<UserLike>()
				.HasOne(src => src.SourceUser)
				.WithMany(target => target.LikedUsers)
				.HasForeignKey(f => f.SourceUserId)
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<UserLike>()
			.HasOne(src => src.TargetUser)
			.WithMany(target => target.LikedByUsers)
			.HasForeignKey(f => f.TargetUserId)
				.OnDelete(DeleteBehavior.NoAction);



		}

	}
}
