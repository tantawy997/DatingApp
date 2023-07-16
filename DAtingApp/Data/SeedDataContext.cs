using CloudinaryDotNet.Actions;
using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DAtingApp.Data
{
	public class SeedDataContext
	{
		public static async Task ClearConnections(DataContext context)
		{
			context.Connections.RemoveRange(context.Connections);
			await context.SaveChangesAsync();
		}
		public static async Task Seed(UserManager<AppUser> userManager,DataContext context,RoleManager<AppRole> roleManager)
		{
			if (await userManager.Users.AnyAsync() 
				//|| await userManager.Users.CountAsync() > 0
				) return;
			
			var userData = await File.ReadAllTextAsync("Data/JsonFiles/UserSeedData.json");
			var users = JsonSerializer.Deserialize<List<AppUser>>(userData);


			//var mac = new HMACSHA512();

			var userphotos = await File.ReadAllTextAsync("Data/JsonFiles/Photos.json");
			var photos = JsonSerializer.Deserialize<List<Photo>>(userphotos);

			var roles = new List<AppRole>

			{ 
				new AppRole{Name = "Admin"},
				new AppRole{Name= "Member"},
				new AppRole{Name = "Moderator"}
			};


			foreach(var role in roles )
			{
				await roleManager.CreateAsync(role);
			}


			for (int i=0; i < users.Count();i++)
			{
				users[i].UserName = users[i].UserName.ToLower();
				users[i].Created = DateTime.SpecifyKind(users[i].Created, DateTimeKind.Utc);
				users[i].LastActive = DateTime.SpecifyKind(users[i].LastActive, DateTimeKind.Utc);
			
				await userManager.CreateAsync(users[i],"Pa$$w0rd");
				await userManager.AddToRoleAsync(users[i], "Member");

				for (int j = 0; j <= i; j++)
				{
					photos[j].IsMain = photos[j].IsMain;
					photos[j].Url = photos[j].Url;
					photos[j].UserId = users[j].Id;

				}

			}
			await context.Photos.AddRangeAsync(photos);

			await context.SaveChangesAsync();

			var admin = new AppUser { UserName = "Admin" };


			await userManager.CreateAsync(admin, "Pa$$w0rd");

			await userManager.AddToRolesAsync(admin, new [] { "Admin", "Moderator" });

		}
	}
}
