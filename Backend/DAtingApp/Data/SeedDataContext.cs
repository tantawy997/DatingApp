using DatingApp.Data;
using DatingApp.Entites;
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
		public static async Task Seed(DataContext context)
		{
			if (await context.Users.AnyAsync()) return;

			var userData = await File.ReadAllTextAsync("Data/JsonFiles/UserSeedData.json");
			var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

			var mac = new HMACSHA512();

			var userphotos = await File.ReadAllTextAsync("Data/JsonFiles/Photos.json");

			var photos = JsonSerializer.Deserialize<List<Photo>>(userphotos);
			for (int i=0; i < users.Count();i++)
			{
				users[i].UserName = users[i].UserName.ToLower();
				users[i].PasswordHah = mac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$word"));
				users[i].PasswordSalt = mac.Key;
				await context.Users.AddAsync(users[i]);
				for (int j =0; j< i;j++)
				{
					photos[j].PhotoId = Guid.NewGuid();
					photos[j].IsMain = photos[j].IsMain;
					photos[j].Url = photos[j].Url;
					photos[j].UserId = users[j].UserId;

					await context.Photos.AddAsync(photos[j]);

				}

			}

			await context.SaveChangesAsync();


		}
	}
}
