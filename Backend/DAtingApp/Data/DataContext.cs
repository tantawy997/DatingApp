using DatingApp.Entites;
using DAtingApp.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;

namespace DatingApp.Data
{
	public class DataContext : IdentityDbContext<AppUser, AppRole,Guid,
		IdentityUserClaim<Guid>, AppUserRole,IdentityUserLogin<Guid>,IdentityRoleClaim<Guid>
		,IdentityUserToken<Guid>>
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}

		public virtual DbSet<Photo> Photos { get; set; }
		public virtual DbSet<UserLike> Likes { get; set; }
		public virtual DbSet<Message> Messages { get; set; }
		public virtual DbSet<Group> Groups { get; set; }
		public virtual DbSet<Connection> Connections { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<AppUser>().
				HasMany(ur => ur.UserRoles)
				.WithOne(u => u.User)
				.HasForeignKey(f => f.UserId)
				.IsRequired();

			modelBuilder.Entity<AppRole>().
			HasMany(ur => ur.UserRoles)
			.WithOne(u => u.Role)
			.HasForeignKey(f => f.RoleId)
			.IsRequired();

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
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<Message>()
				.HasOne(send => send.Recipient)
				.WithMany(message => message.MessagesReceived)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Message>()
				.HasOne(u => u.Sender)
				.WithMany(u => u.MessagesSent)
				.OnDelete(DeleteBehavior.Restrict);


				



		}

	}
}
